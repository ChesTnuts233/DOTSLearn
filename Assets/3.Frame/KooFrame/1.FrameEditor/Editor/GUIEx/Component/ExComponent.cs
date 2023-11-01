// using UnityEditor;
// using UnityEngine;
//
// /// <summary>
// /// 拓展原生Transform
// /// </summary>
// [CustomEditor(typeof(Transform))]
// public class ExComponent : Editor
// {
//     //private Editor m_Editor;
//     private static bool m_FoldoutState = true;
//     public static bool IsShowTranformHelp = true;
//     public static bool IsTransformUse = true;
//
//
//     // private void OnEnable()
//     // {
//     //     m_Editor = Editor.CreateEditor(target,
//     //         Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.TransformInspector", false));
//     // }
//
//     public override void OnInspectorGUI()
//     {
//         if (IsShowTranformHelp) //是否启用
//         {
//             GUI.enabled = IsTransformUse;
//             // 绘制折叠框
//             m_FoldoutState = EditorGUILayout.Foldout(m_FoldoutState, "KooTranformHelp");
//             if (m_FoldoutState)
//             {
//                 GUILayout.BeginVertical();
//                 GUILayout.BeginHorizontal();
//                 GUILayout.Space(10);
//
//                 #region 本地重置
//
//                 GUILayout.BeginVertical();
//
//                 if (GUILayout.Button("本地坐标重置"))
//                 {
//                     var obj = (Transform)target;
//                     obj.transform.localPosition = Vector3.zero;
//                     Debug.Log("KooHelp:" + "此物体的LocalPosition已经归零啦！");
//                 }
//
//                 GUILayout.Space(1);
//                 if (GUILayout.Button("本地旋转重置"))
//                 {
//                     var obj = (Transform)target;
//                     obj.transform.localRotation = Quaternion.identity;
//                     Debug.Log("KooHelp:" + "此物体的LocalRotation已经归零啦！");
//                 }
//
//                 GUILayout.EndVertical();
//
//                 #endregion
//
//                 GUILayout.Space(20);
//
//                 #region 世界重置
//
//                 GUILayout.BeginVertical();
//
//                 if (GUILayout.Button("世界坐标重置"))
//                 {
//                     var obj = (Transform)target;
//                     obj.transform.position = Vector3.zero;
//                     Debug.Log("KooHelp:" + "此物体已到跑到世界原点位置啦！");
//                 }
//
//                 GUILayout.Space(1);
//                 if (GUILayout.Button("世界旋转重置"))
//                 {
//                     var obj = (Transform)target;
//                     obj.transform.rotation = Quaternion.identity;
//                     Debug.Log("KooHelp:" + "此物体相对世界的角度已经归零啦！");
//                 }
//
//                 GUILayout.EndVertical();
//
//                 #endregion
//
//                 GUILayout.Space(10);
//                 GUILayout.EndHorizontal();
//
//                 #region 尺寸设置
//
//                 GUILayout.BeginHorizontal();
//                 GUILayout.Space(10);
//                 if (GUILayout.Button("尺寸归一"))
//                 {
//                     var obj = (Transform)target;
//                     obj.transform.localScale = Vector3.one;
//                     Debug.Log("KooHelp:" + "此物体本地尺寸已经都变1啦！");
//                 }
//
//                 GUILayout.Space(20);
//                 if (GUILayout.Button("尺寸乘十"))
//                 {
//                     var obj = (Transform)target;
//                     obj.transform.localScale *= 10;
//                     Debug.Log("KooHelp:" + "此物体本地尺寸变大10倍啦！");
//                 }
//
//                 GUILayout.Space(20);
//                 if (GUILayout.Button("尺寸除十"))
//                 {
//                     var obj = (Transform)target;
//                     obj.transform.localScale /= 10;
//                     Debug.Log("KooHelp:" + "此物体本地尺寸变小10倍啦！");
//                 }
//
//                 GUILayout.Space(10);
//                 GUILayout.EndHorizontal();
//
//                 #endregion
//
//                 GUILayout.EndVertical();
//             }
//
//             base.OnInspectorGUI();
//         }
//         else
//         {
//             GUI.enabled = IsTransformUse;
//             base.OnInspectorGUI();
//         }
//     }
//
//     [MenuItem("CONTEXT/Transform/KooTransformHelp/开关KooTransformHelp")]
//     public static void KooTransformHelp()
//     {
//         IsShowTranformHelp = !IsShowTranformHelp;
//     }
//
//     [MenuItem("CONTEXT/Transform/KooTransformHelp/是否禁用Transform编辑")]
//     public static void KooTransformForbid()
//     {
//         IsTransformUse = !IsTransformUse;
//     }
//
//     ////这里是利用C#反射 虽然有好处是可以展现原生界面 但删除物品的时候会有红错 而且有过m_Editor报空  P40 3-11 
//     //private Editor m_Editor;
//     //private void OnEnable()
//     //{
//     //    m_Editor = Editor.CreateEditor(target,
//     //        Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.TransformInspector", true));
//     //}
//     //public override void OnInspectorGUI()
//     //{
//     //    if (GUILayout.Button("只归零位置"))
//     //    {
//     //        var obj = (Transform)target;
//     //        obj.transform.position = Vector3.zero;
//     //        Debug.Log("位置已经归零啦！");
//     //    }
//     //    m_Editor.OnInspectorGUI();
//
//     //    ////开始禁止 组件不可编辑
//     //    //GUI.enabled = false;
//     //    //m_Editor.OnInspectorGUI();
//
//     //    ////结束禁止
//     //    //GUI.enabled = true;
//     //}
// }