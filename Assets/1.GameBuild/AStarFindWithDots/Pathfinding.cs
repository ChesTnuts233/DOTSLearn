//****************** 代码文件申明 ************************
//* 文件：Pathfinding                                       
//* 作者：#AUTHORNAME#
//* 创建时间：#CREATETIME#
//* 描述：Nothing进行
//*****************************************************

using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace GameBuild
{
    public class Pathfinding : MonoBehaviour
    {
        //test
        private void Start()
        {
            FindPath(new int2(0, 0), new int2(3, 1));
        }


        #region AStart基本数据结构

        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        private struct PathNode
        {
            public int X;
            public int Y;

            public int Index;

            /// <summary>
            /// gCost为 当前格(走到的地方) 距离 起点 的成本
            /// </summary>
            public int GCost;

            /// <summary>
            /// hCost为 当前格(走到的地方) 到 终点 的距离成本
            /// </summary>
            public int HCost;

            /// <summary>
            /// 总计成本 为 G + H
            /// </summary>
            public int FCost;

            /// <summary>
            /// 是否可以行走
            /// </summary>
            public bool IsWalkable;

            /// <summary>
            /// 上一个走过来的节点
            /// </summary>
            public int CameFromNodeIndex;

            public void CalculateFCost()
            {
                FCost = GCost + HCost;
            }
        }

        #endregion


        private void FindPath(int2 startPosition, int2 endPosition)
        {
            int2 gridSize = new int2(4, 4);

            NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    PathNode pathNode = new PathNode();
                    pathNode.X = x;
                    pathNode.Y = y;

                    pathNode.Index = CalculateIndex(x, y, gridSize.x);

                    //gCost为 当前格(走到的地方) 距离 起点 的成本
                    pathNode.GCost = int.MaxValue;
                    //hCost为 当前格(走到的地方) 到终点的距离成本
                    pathNode.HCost = CalculateDistanceCost(new int2(x, y), endPosition);
                    pathNode.CalculateFCost();

                    pathNode.IsWalkable = true;
                    pathNode.CameFromNodeIndex = -1;

                    pathNodeArray[pathNode.Index] = pathNode;
                }
            }

            //新建邻居数组
            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(new int2[]
            {
                new int2(-1, 0),  //左
                new int2(+1, 0),  //右
                new int2(0, +1),  //上
                new int2(0, -1),  //下
                new int2(-1, -1), //左下
                new int2(-1, +1), //左上
                new int2(+1, -1), //右下
                new int2(+1, +1), //右上 
            }, Allocator.Temp);


            int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, gridSize.x);


            PathNode startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y, gridSize.x)];
            startNode.GCost = 0;
            startNode.CalculateFCost();
            //因为这里修改的都是值类型 所以要把"副本" 赋值回原数组
            pathNodeArray[startNode.Index] = startNode;

            //开放列表是用来 存储还没探索的节点或者路径段 这是一个优先级列表 节点按照估计的总代价进行排序 以便于探索具有最小fCost的节点
            NativeList<int> openList = new NativeList<int>(Allocator.Temp);

            //封闭列表是用来 存储已经探索过的节点或者路径段 以避免重复探索相同的节点 一旦一个节点被从 开放列表 中选择并 标记 它会被移到封闭列表中
            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

            //开始节点的索引加入到开发列表中
            openList.Add(startNode.Index);

            //当还没探索完的节点数量依旧大于0的时候
            while (openList.Length > 0)
            {
                //当前节点索引为最小FCost
                int currentNodeIndex = GetLowestFCostNodeIndex(openList, pathNodeArray);
                //获取节点
                PathNode currentNode = pathNodeArray[currentNodeIndex];

                //如果已经到达终点
                if (currentNodeIndex == endNodeIndex)
                {
                    break;
                }

                //已经探索完当前节点了 移出出开放列表
                for (int i = 0; i < openList.Length; i++)
                {
                    if (openList[i] == currentNodeIndex)
                    {
                        //把此列表的最后一个元素复制到指定的索引 长度-1
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }

                //添加进封闭列表
                closedList.Add(currentNodeIndex);

                for (int i = 0; i < neighbourOffsetArray.Length; i++)
                {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition =
                        new int2(currentNode.X + neighbourOffset.x, currentNode.Y + neighbourOffset.y);

                    //如果节点不在网格内
                    if (!IsPositionInsideGrid(neighbourPosition, gridSize))
                    {
                        continue;
                    }

                    int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, gridSize.x);

                    //如果已经搜索过这个邻居节点
                    if (closedList.Contains(neighbourNodeIndex))
                        continue;

                    PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
                    //如果不能走
                    if (!neighbourNode.IsWalkable)
                        continue;

                    int2 currentNodePosition = new int2(currentNode.X, currentNode.Y);
                    //暂定的GCost
                    int tentativeGCost =
                        currentNode.GCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);

                    //如果新的GCost小于当前G成本 
                    if (tentativeGCost < neighbourNode.GCost)
                    {
                        //更新上一个走过的节点
                        neighbourNode.CameFromNodeIndex = currentNodeIndex;
                        //更新邻居节点的GCost
                        neighbourNode.GCost = tentativeGCost;
                        //更新它的FCost
                        neighbourNode.CalculateFCost();
                        //因为是值类型 更新完节点后 放回进数组中
                        pathNodeArray[neighbourNodeIndex] = neighbourNode;
                        //如果开放列表没有这个邻居节点 则加进开放列表
                        if (!openList.Contains(neighbourNode.Index))
                        {
                            openList.Add(neighbourNode.Index);
                        }
                    }
                }
            }

            PathNode endNode = pathNodeArray[endNodeIndex];
            if (endNode.CameFromNodeIndex == -1) //没有找到路径
            {
                Debug.Log("没能找到路径");
            }
            else //找到路径
            {
                NativeList<int2> path = CalculatePath(pathNodeArray, endNode);
                foreach (int2 pathPosition in path)
                {
                    Debug.Log(pathPosition);
                }

                path.Dispose();
            }

            pathNodeArray.Dispose();
            neighbourOffsetArray.Dispose();
            openList.Dispose();
            closedList.Dispose();
        }

        #region AStart内部计算

        private NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode)
        {
            if (endNode.CameFromNodeIndex == -1) //未能找到路径
            {
                return new NativeList<int2>(Allocator.Temp);
            }
            else //找到了路径
            {
                NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                path.Add(new int2(endNode.X, endNode.Y));

                PathNode currentNode = endNode;
                //需要向后遍历才能找到移动路径
                while (currentNode.CameFromNodeIndex != -1)
                {
                    PathNode cameFromNode = pathNodeArray[currentNode.CameFromNodeIndex];
                    path.Add(new int2(cameFromNode.X, cameFromNode.Y));
                    currentNode = cameFromNode;
                }

                //返回一个反向的路径
                return path;
            }
        }


        /// <summary>
        /// 当前节点是否在网格内
        /// </summary>
        /// <param name="gridPosition">节点坐标</param>
        /// <param name="gridSize">网格尺寸</param>
        /// <returns>true就是在内部</returns>
        private bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize)
        {
            return gridPosition.x >= 0 &&
                   gridPosition.y >= 0 &&
                   gridPosition.x < gridSize.x &&
                   gridPosition.y < gridSize.y;
        }

        /// <summary>
        /// 计算索引值
        /// </summary>
        /// <param name="x">当前网格x值</param>
        /// <param name="y">当前网格y值</param>
        /// <param name="gridWidth">当前网格宽度</param>
        /// <returns></returns>
        private int CalculateIndex(int x, int y, int gridWidth)
        {
            return x + y * gridWidth;
        }

        /// <summary>
        /// 计算A点到B点的距离成本
        /// </summary>
        /// <param name="aPosition"></param>
        /// <param name="bPosition"></param>
        /// <returns></returns>
        private int CalculateDistanceCost(int2 aPosition, int2 bPosition)
        {
            int xDistance = math.abs(aPosition.x - bPosition.x); //x轴距离
            int yDistance = math.abs(aPosition.y - bPosition.y); //y轴距离
            int remaining = math.abs(xDistance - yDistance);     //差值
            //距离成本 = 14(斜角距离1.4 x 10)  * 短边 + 10(1 * 10 直线距离) * 差值
            return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        /// <summary>
        /// 获取最小fCost的节点的索引
        /// </summary>
        /// <param name="openList">开放列表</param>
        /// <param name="pathNodeArray">周围的节点</param>
        /// <returns></returns>
        private int GetLowestFCostNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray)
        {
            PathNode lowestFCostPathNode = pathNodeArray[openList[0]];
            //从第一个节点开发遍历开放列表 如果数组值的节点值有小于其中的 则最小值更新为它
            for (int i = 1; i < openList.Length; i++)
            {
                PathNode tempPathNode = pathNodeArray[openList[i]];
                if (tempPathNode.FCost < lowestFCostPathNode.FCost)
                {
                    lowestFCostPathNode = tempPathNode;
                }
            }

            return lowestFCostPathNode.Index;
        }

        #endregion
    }
}