using System;
using System.Collections.Generic;
using KooFrame;
using UnityEngine;

namespace GameBuild.SubSystem
{
    /// <summary>
    /// 可序列化的贝塞尔曲线 
    /// </summary>
    [Serializable]
    public class BezierCurveSerializable
    {
        // /// <summary>
        // /// 贝塞尔曲线类型
        // /// </summary>
        // [SerializeField]
        // private BezierType _bezierType = BezierType.Three;
        //
        // /// <summary>
        // /// 使用类型
        // /// </summary>
        // [SerializeField]
        // private BezierUseType _userType;

        /// <summary>
        /// 使用二维的时候可以使用编辑器来定义曲线
        /// </summary>
        [SerializeField, HideIfNull]
        public AnimationCurve widthCurve;

        /// <summary>
        /// 段数
        /// </summary>
        [Range(1, 100)]
        public int segments = 10;

        /// <summary>
        /// 是否循环
        /// </summary>
        public bool loop;

        /// <summary>
        /// 点集合
        /// 默认初始化两个节点
        /// </summary>
        public List<BezierCurvePoint> points = new List<BezierCurvePoint>(2)
        {
            new(position: Vector2.left * 5f, tangent: Vector2.left * 5f + Vector2.up * 3f),
            new(position: Vector3.right * 5f, tangent: Vector3.right * 5f + Vector3.up * 3f)
        };


        /// <summary>
        /// 根据归一化位置值获取对应的贝塞尔曲线上的点
        /// </summary>
        /// <param name="value">归一化位置值 [0,1]</param>
        /// <returns></returns>
        public Vector3 EvaluatePosition(float value)
        {
            Vector3 retVal = Vector3.zero;
            if (points.Count > 0)
            {
                float max = points.Count - 1 < 1 ? 0 : (loop ? points.Count : points.Count - 1);
                float standardized = (loop && max > 0)
                    ? ((value %= max) + (value < 0 ? max : 0))
                    : Mathf.Clamp(value, 0, max);
                int rounded = Mathf.RoundToInt(standardized);
                int i1, i2;


                #region 什么是Mathf.Epsilon

                // Mathf.Epsilon  是一个大于0的最小浮点数
                // 此函数遵循以下规则：
                // anyValue + Epsilon = anyValue
                // anyValue - Epsilon = anyValue
                // 0 + Epsilon = Epsilon
                // 0 - Epsilon = -Epsilon
                // 任意数 与 Epsilon 的之间值将导致在任意数发生舍位误差（truncating errors）

                #endregion

                if (Mathf.Abs(standardized - rounded) < Mathf.Epsilon)
                {
                    i1 = i2 = (rounded == points.Count) ? 0 : rounded;
                }
                else
                {
                    i1 = Mathf.FloorToInt(standardized);
                    if (i1 >= points.Count)
                    {
                        standardized -= max;
                        i1 = 0;
                    }

                    i2 = Mathf.CeilToInt(standardized);
                    i2 = i2 >= points.Count ? 0 : i2;
                }

                if (i1 == i2)
                    retVal = points[i1].position;
                else
                    retVal = KooGraphicsTool.Bezier3(points[i1].position,
                        points[i1].position + points[i1].tangent, points[i2].position
                                                                  + points[i2].tangent, points[i2].position,
                        standardized - i1);
            }

            return retVal;
        }
    }
}