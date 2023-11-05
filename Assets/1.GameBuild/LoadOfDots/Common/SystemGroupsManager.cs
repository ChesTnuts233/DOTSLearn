//****************** 代码文件申明 ************************
//* 文件：AuthoringSceneSystemGroup                                       
//* 作者：#AUTHORNAME#
//* 创建时间：#CREATETIME#
//* 描述：Nothing
//*****************************************************

using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GameBuild.DOTS
{
    public abstract partial class AuthoringSceneSystemGroup : ComponentSystemGroup
    {
        /// <summary>
        /// 是否初始化
        /// </summary>
        private bool _initialized;

        protected override void OnCreate()
        {
            base.OnCreate();
            _initialized = false;
        }


        protected override void OnUpdate()
        {
            if (!_initialized)
            {
                if (SceneManager.GetActiveScene().isLoaded)
                {
                    var subScene = Object.FindObjectOfType<SubScene>();
                    if (subScene != null)
                    {
                        //Enabled 决定了该子场景中的System是否执行
                        //如果申明的AuthoringSceneName 为当前子场景的scene名称 则执行
                        Enabled = AuthoringSceneName == subScene.gameObject.scene.name;
                    }
                }
                else
                {
                    Enabled = false;
                }

                //初始化完成
                _initialized = true;
            }
            base.OnUpdate();
        }
        
        /// <summary>
        /// 系统执行场景名称
        /// </summary>
        protected abstract string AuthoringSceneName { get; }
    }
 }

