#if UNITY_EDITOR


using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace GameBuild.SubSystem
{
    [CustomEditor(typeof(SimpleBezierCurvePath))]
    public class SimpleBezierCurvePathEditor : OdinEditor
    {
        /// <summary>
        /// 贝塞尔曲线路径
        /// </summary>
        private SimpleBezierCurvePath path;

        /// <summary>
        /// Gizmos球体尺寸
        /// </summary>
        private const float sphereHandleCapSize = 0.2f;

        protected override void OnEnable()
        {
            base.OnEnable();
            path = target as SimpleBezierCurvePath;
        }

        private void OnSceneGUI()
        {
            //路径点集合为空
            if (path.Points == null || path.Points.Count == 0) return;

            //当前选中工具非移动工具
            if (Tools.current != Tool.Move) return;
            //颜色缓存
            Color cacheColor = Handles.color;
            Handles.color = Color.red;

            //遍历路径点集合
            for (var i = 0; i < path.Points.Count; i++)
            {
                DrawPositionHandle(i);
                DrawTangentHandle(i);

                BezierCurvePoint point = path.Points[i];
                //路径点、控制点的本地坐标转世界坐标 
                Vector3 position = path.transform.TransformPoint(point.position);
                Vector3 controlPoint = path.transform.TransformPoint(point.position + point.tangent);

                point.angle = (path.transform.TransformPoint(point.tangent) -
                               path.transform.TransformPoint(point.position)).normalized;


                //绘制切线
                Handles.DrawDottedLine(position, controlPoint, 1f);
            }

            //恢复颜色
            Handles.color = cacheColor;
        }

        /// <summary>
        /// 路径点操作柄绘制
        /// </summary>
        /// <param name="index"></param>
        private void DrawPositionHandle(int index)
        {
            BezierCurvePoint point = path.Points[index];
            //本地坐标转世界
            Vector3 position = path.transform.TransformPoint(point.position);
            //操作柄的旋转类型
            Quaternion rotation = Tools.pivotRotation == PivotRotation.Local
                ? path.transform.rotation
                : Quaternion.identity;
            //操作柄的大小
            float size = HandleUtility.GetHandleSize(position) * sphereHandleCapSize;
            //在该路径点绘制一个球形
            Handles.color = Color.white;
            Handles.SphereHandleCap(0, position, rotation, size, EventType.Repaint);
            Handles.Label(position, $"Point{index}");
            //检测变更
            EditorGUI.BeginChangeCheck();
            //坐标操作柄
            position = Handles.PositionHandle(position, rotation);
            //变更检测结束 如果发生变更 更新路径点
            if (EditorGUI.EndChangeCheck())
            {
                //记录操作
                Undo.RecordObject(path, "Position Changed");
                //全局转局部坐标
                point.position = path.transform.InverseTransformPoint(position);
                //更新路径点
                path.Points[index] = point;
            }
        }

        /// <summary>
        /// 操作点控制柄绘制
        /// </summary>
        /// <param name="index">操作点序号</param>
        private void DrawTangentHandle(int index)
        {
            BezierCurvePoint point = path.Points[index];
            //本地转世界坐标
            Vector3 tangentPos = path.transform.TransformPoint(point.position + point.tangent);
            //操作柄的旋转类型
            Quaternion rotation = Tools.pivotRotation == PivotRotation.Local
                ? path.transform.rotation
                : Quaternion.identity;
            //操作柄的大小
            float size = HandleUtility.GetHandleSize(tangentPos) * sphereHandleCapSize;
            //在该控制点绘制一个球形
            Handles.color = Color.yellow;
            Handles.SphereHandleCap(0, tangentPos, rotation, size, EventType.Repaint);
            //检测变更
            EditorGUI.BeginChangeCheck();
            //操作柄坐标
            tangentPos = Handles.PositionHandle(tangentPos, rotation);
            //变更检测结束 如果发生变更 更新路径点
            if (EditorGUI.EndChangeCheck())
            {
                //记录操作
                Undo.RecordObject(path, "Control Point Changed");
                //世界坐标转本地坐标
                point.tangent = path.transform.InverseTransformPoint(tangentPos) - point.position;
                //更新路径点
                path.Points[index] = point;
            }
        }
    }
}
#endif