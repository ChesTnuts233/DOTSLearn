using UnityEngine;

namespace KooFrame
{
    public static class KooGraphicsTool
    {
        #region 贝塞尔曲线

        /// <summary>
        /// 一阶贝塞尔曲线
        /// </summary>
        /// <remarks>
        /// B(t) = P0 + (P1 - P0) t = (1 - t) P0 + t P1, t ∈ [0, 1]
        /// </remarks>
        /// <param name="p0">起点</param>
        /// <param name="p1">终点</param>
        /// <param name="t">[0,1]</param>
        public static Vector3 Bezier1(Vector3 p0, Vector3 p1, float t)
        {
            return (1 - t) * p0 + t * p1;
        }

        /// <summary>
        /// 二阶贝塞尔曲线
        /// </summary>
        /// <remarks>
        /// B(t) = (1 - t)^2 P0 + 2t (1 - t) P1 + t^2 * P2, t ∈[0, 1]
        /// </remarks> 
        /// <param name="p0">起点</param>
        /// <param name="p1">控制点1</param>
        /// <param name="p2">终点</param>
        /// <param name="t">[0,1]</param>
        public static Vector3 Bezier2(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            Vector3 p0p1 = (1 - t) * p0 + t * p1;
            Vector3 p1p2 = (1 - t) * p1 + t * p2;
            return (1 - t) * p0p1 + t * p1p2;
        }

        /// <summary>
        /// 三阶贝塞尔曲线
        /// </summary>
        /// <remarks>
        /// B(t) = P0(1 - t)^3 + 3P1t(1 - t)^2 + 3P2t^2(1 - t) + P3t^3, t ∈ [0, 1]
        /// </remarks>
        /// <param name="p0">起点</param>
        /// <param name="p1">控制点1</param>
        /// <param name="p2">控制点2</param>
        /// <param name="p3">终点</param>
        /// <param name="t">[0,1]</param>
        public static Vector3 Bezier3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            Vector3 p0p1 = (1 - t) * p0 + t * p1;
            Vector3 p1p2 = (1 - t) * p1 + t * p2;
            Vector3 p2p3 = (1 - t) * p2 + t * p3;
            Vector3 p0p1p2 = (1 - t) * p0p1 + t * p1p2;
            Vector3 p1p2p3 = (1 - t) * p1p2 + t * p2p3;
            return (1 - t) * p0p1p2 + t * p1p2p3;
        }

        #endregion
    }
}