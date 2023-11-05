//****************** 代码文件申明 ************************
//* 文件：ShowConnect                                       
//* 作者：#AUTHORNAME#
//* 创建时间：#CREATETIME#
//* 描述：Nothing
//*****************************************************

using UnityEngine;
using UnityEditor;

namespace GameBuild
{
    [CustomEditor(typeof(FixedJoint2D))]
    public class ShowConnect : Editor
    {
        private FixedJoint2D _fixedJoint2D;

        private void OnEnable()
        {
            _fixedJoint2D = target as FixedJoint2D;
        }

        private void OnSceneGUI()
        {
            var pos = _fixedJoint2D.connectedBody.transform.position;
            Handles.color = Color.red;
            Quaternion rotation = Tools.pivotRotation == PivotRotation.Local
                ? _fixedJoint2D.transform.rotation
                : Quaternion.identity;
            Handles.SphereHandleCap(0, pos, rotation, 0.2f, EventType.Repaint);
        }
    }
}