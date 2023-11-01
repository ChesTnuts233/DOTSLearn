using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameBuild.SubSystem
{
    /// <summary>
    /// 简单贝塞尔曲线路径
    /// </summary>
    public class SimpleBezierCurvePath : MonoBehaviour
    {
        [SerializeField]
        private BezierCurveSerializable curve;

        public bool Loop => Curve.loop;

        public List<BezierCurvePoint> Points => Curve.points;

        public BezierCurveSerializable Curve
        {
            get => curve;
            set => curve = value;
        }


        [SerializeField, LabelText("序列化的坐标集合")]
        public List<Vector2> Poslist = null;


        /// <summary>
        /// 根据归一化位置值获取对应的贝塞尔曲线上的点
        /// </summary>
        /// <param name="t">归一化位置值 [0,1]</param>
        /// <returns></returns>
        public Vector3 EvaluatePosition(float t)
        {
            return Curve.EvaluatePosition(t);
        }


        /// <summary>
        /// 生成坐标集合
        /// </summary>
        /// <param name="segments">段数</param>
        [Button("生成坐标集合")]
        public void GeneterPositionByEvaluate()
        {
            Poslist = new List<Vector2>();
            float stepSize = 1f / Curve.segments;
            for (int i = 0; i < Curve.segments; i++)
            {
                Poslist.Add(EvaluatePosition(i * stepSize));
            }
        }


        /// <summary>
        /// 更新坐标集合
        /// </summary>
        [Button("更新坐标集合")]
        public void UpdatePositionByEvaluate()
        {
            Poslist.Clear();
            float stepSize = 1f / Curve.segments;
            for (int i = 0; i < Curve.segments; i++)
            {
                Poslist.Add(EvaluatePosition(i * stepSize));
            }
        }

        /// <summary>
        /// 返回坐标集合
        /// </summary>
        public List<Vector2> GetPositionByEvaluate()
        {
            List<Vector2> posList = new List<Vector2>();
            float stepSize = 1f / Curve.segments;
            for (int i = 0; i < Curve.segments; i++)
            {
                posList.Add(EvaluatePosition(i * stepSize));
            }

            return posList;
        }


#if UNITY_EDITOR
        /// <summary>
        /// 路径颜色(Gizmos)
        /// </summary>
        public Color pathColor = Color.green;


        private void OnDrawGizmos()
        {
            if (Curve == null || Curve.points.Count == 0)
                return;
            //缓存颜色
            Color cacheColor = Gizmos.color;
            //路径绘制颜色
            Gizmos.color = pathColor;
            //步长
            float step = 1f / Curve.segments;
            //缓存上个坐标点
            Vector3 lastPos = transform.TransformPoint(Curve.EvaluatePosition(0f));
            float end = (Curve.points.Count - 1 < 1 ? 0 : (Curve.loop ? Curve.points.Count : Curve.points.Count - 1)) +
                        step * .5f;
            for (float t = step; t <= end; t += step)
            {
                //计算位置
                Vector3 p = transform.TransformPoint(Curve.EvaluatePosition(t));
                //绘制曲线
                Gizmos.DrawLine(lastPos, p);
                //记录
                lastPos = p;
            }


            //恢复颜色
            Gizmos.color = cacheColor;
        }
#endif
    }
}