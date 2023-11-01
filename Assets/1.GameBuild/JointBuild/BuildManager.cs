//****************** 代码文件申明 ************************
//* 文件：BuildManager                                       
//* 作者：32867
//* 创建时间：2023/10/27 15:38:34 星期五
//* 描述：Nothing
//*****************************************************

using UnityEngine;
using KooFrame.BaseSystem;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Internal;
using Unity.VisualScripting;
using UnityEditor;

namespace GameBuild
{
    public class BuildHelp : MonoBehaviour
    {
        /// <summary>
        /// 生成的数量
        /// </summary>
        private int BuildNum;

        [SerializeField]
        private GameObject BuildBox;
        
        
        
        

        /// <summary>
        /// 向上生成的Box
        /// </summary>
        public List<GameObject> box;


        [Button]
        private void Generator(Vector3 dir)
        {
            if (dir == Vector3.zero)
            {
                dir = Vector2.up;
            }

            var target = GameObject.Instantiate(BuildBox, transform.position + dir, Quaternion.identity);

            target.name = "Box" + target.transform.position;
            FixedJoint2D thisjoint;
            if (!TryGetComponent(out thisjoint))
            {
                thisjoint = this.AddComponent<FixedJoint2D>();
            }

            FixedJoint2D targetJoint;
            if (!target.TryGetComponent(out targetJoint))
            {
                targetJoint = target.AddComponent<FixedJoint2D>();
            }

            targetJoint.connectedBody = this.GetComponent<Rigidbody2D>();
            thisjoint.connectedBody = target.GetComponent<Rigidbody2D>();
            Selection.activeGameObject = target;
        }
    }
}