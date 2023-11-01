using System;
using UnityEngine;

namespace GameBuild.SubSystem
{
    [Serializable]
    public struct BezierCurvePoint
    {
        /// <summary>
        /// 坐标点
        /// </summary>
        public Vector3 position;

        /// <summary>
        /// 控制点 与坐标点形成切线
        /// </summary>
        public Vector3 tangent;

        public Vector3 angle;

        public BezierCurvePoint(Vector3 position, Vector3 tangent)
        {
            this.position = position;
            this.tangent = tangent;
            angle = (this.tangent - this.position).normalized;
        }
    }
}