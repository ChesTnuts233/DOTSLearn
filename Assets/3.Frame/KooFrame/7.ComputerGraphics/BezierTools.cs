//****************** 代码文件申明 ************************
//* 文件：BezierManager                                       
//* 作者：32867
//* 创建时间：2023/10/18 16:55:03 星期三
//* 描述：贝塞尔曲线公式
//*****************************************************

using System.Collections.Generic;
using GameBuild.SubSystem;
using UnityEngine;

namespace KooFrame
{
    public static class BezierTools
    {
        /// <summary>
        /// 生成简单的贝塞尔曲线
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="segments">曲线段数 默认为10</param>
        /// <returns>一段简单曲线</returns>
        public static BezierCurve CreateSimpleBezier3(BezierCurvePoint startPoint, BezierCurvePoint endPoint,
            int segments = 10)
        {
            return new BezierCurve(startPoint, endPoint, segments);
        }


        /// <summary>
        /// 生成简单的贝塞尔曲线坐标集合
        /// </summary>
        /// <param name="startPoint">起点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="segments">曲线段数 默认为10</param>
        /// <returns></returns>
        public static List<Vector3> CreateSimpleBezierPosList(BezierCurvePoint startPoint, BezierCurvePoint endPoint,
            int segments = 10)
        {
            List<Vector3> posList = new List<Vector3>();
            BezierCurve curve = new BezierCurve(startPoint, endPoint, segments);
            float stepSize = 1f / curve.segments;
            for (int i = 0; i < curve.segments; i++)
            {
                posList.Add(curve.EvaluatePosition(i * stepSize));
            }

            return posList;
        }

        /// <summary>
        /// 根据坐标来生成贝塞尔曲线坐标集合
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="startTangent">起始切点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="endTangent">终点切点</param>
        /// <param name="segments">段数 默认为10</param>
        /// <returns></returns>
        public static List<Vector3> CreateSimpleBezierPosList(Vector3 startPoint, Vector3 startTangent,
            Vector3 endPoint, Vector3 endTangent, int segments = 10)
        {
            List<Vector3> posList = new List<Vector3>();
            var point1 = new BezierCurvePoint() { position = startPoint, tangent = startTangent };
            var point2 = new BezierCurvePoint() { position = endPoint, tangent = endTangent };
            BezierCurve curve = new BezierCurve(point1, point2, segments);
            float stepSize = 1f / curve.segments;
            for (int i = 0; i < curve.segments; i++)
            {
                posList.Add(curve.EvaluatePosition(i * stepSize));
            }

            return posList;
        }

        /// <summary>
        /// 根据坐标来生成贝塞尔曲线坐标集合
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="startTangent">起始切点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="endTangent">终点切点</param>
        /// <param name="segments">段数 默认为10</param>
        /// <returns></returns>
        public static List<Vector2> CreateSimpleBezierPosList(Vector2 startPoint, Vector2 startTangent,
            Vector2 endPoint, Vector2 endTangent, int segments = 10)
        {
            List<Vector2> posList = new List<Vector2>();
            var point1 = new BezierCurvePoint() { position = startPoint, tangent = startTangent };
            var point2 = new BezierCurvePoint() { position = endPoint, tangent = endTangent };
            BezierCurve curve = new BezierCurve(point1, point2, segments);
            float stepSize = 1f / curve.segments;
            for (int i = 0; i < curve.segments; i++)
            {
                posList.Add(curve.EvaluatePosition(i * stepSize));
            }

            return posList;
        }

        /// <summary>
        /// 根据默认的切线来生成曲线坐标集合
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="segments">段数 默认为10</param>
        /// <returns>返回的是一条两边各加曲线Vector3.up/down * 5f的曲率</returns>
        public static List<Vector3> CreateSimpleBezierPosList(Vector3 startPoint, Vector3 endPoint, int segments = 10)
        {
            List<Vector3> posList = new List<Vector3>();
            Vector3 startTangent = startPoint + Vector3.up * 5f;
            Vector3 endTangent = startPoint + Vector3.down * 5f;
            var point1 = new BezierCurvePoint() { position = startPoint, tangent = startTangent };
            var point2 = new BezierCurvePoint() { position = endPoint, tangent = endTangent };
            BezierCurve curve = new BezierCurve(point1, point2, segments);
            float stepSize = 1f / curve.segments;
            for (int i = 0; i < curve.segments; i++)
            {
                posList.Add(curve.EvaluatePosition(i * stepSize));
            }

            return posList;
        }

        /// <summary>
        /// 根据默认的切线来生成曲线坐标集合
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="segments">段数 默认为10</param>
        /// <returns>返回的是一条两边各加曲线Vector3.up/down * 5f的曲率</returns>
        public static List<Vector2> CreateSimpleBezierPosList(Vector2 startPoint, Vector2 endPoint, int segments = 10)
        {
            List<Vector2> posList = new List<Vector2>();
            Vector2 startTangent = startPoint + Vector2.up * 5f;
            Vector2 endTangent = startPoint + Vector2.down * 5f;
            var point1 = new BezierCurvePoint() { position = startPoint, tangent = startTangent };
            var point2 = new BezierCurvePoint() { position = endPoint, tangent = endTangent };
            BezierCurve curve = new BezierCurve(point1, point2, segments);
            float stepSize = 1f / curve.segments;
            for (int i = 0; i < curve.segments; i++)
            {
                posList.Add(curve.EvaluatePosition(i * stepSize));
            }

            return posList;
        }


        /// <summary>
        /// 生成一条随机的贝塞尔曲线
        /// </summary>
        public static BezierCurve CreateRandomBezier3(int segments = 10)
        {
            return new BezierCurve(
                new BezierCurvePoint(Vector2.left * 5f * Random.Range(1, 10),
                    Vector2.left * 5f + Vector2.up * 3f * Random.Range(1, 10)),
                new BezierCurvePoint(Vector2.right * 5f * Random.Range(1, 10),
                    Vector2.right * 5f + Vector2.up * 3f * Random.Range(1, 10)), segments);
           
        }
    }
}