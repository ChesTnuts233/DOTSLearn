﻿using UnityEngine;

namespace KooFrame
{
    /// <summary>
    /// 提供的自动Mono单例类
    /// 自动放置于DontDestroy
    /// 这是一种懒汉单例
    /// </summary>
    /// <typeparam name="T">单例泛型</typeparam>
    public abstract class KooAutoMonoSingleton<T> : MonoBehaviour where T : KooAutoMonoSingleton<T>
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                //不空直接返回单例
                if (instance is not null) 
                {
                    Debug.LogFormat("单例类 {0} 已经存在", instance.name);
                    return instance;
                }
                //在场景中寻找是否挂有此脚本
                instance = FindFirstObjectByType<T>();

                if (FindObjectsByType<T>(FindObjectsSortMode.None).Length > 1)
                {
                    Debug.LogWarning("已超过一个单例 [There is more than one instance]");
                    return instance;
                }

                if (instance is null)
                {
                    var instanceName = typeof(T).Name;
                    Debug.LogFormat("单例名: {0}", instanceName);
                    GameObject instanceObj = GameObject.Find(instanceName);
                    if (!instanceObj)
                    {
                        instanceObj = new GameObject(instanceName);
                    }

                    instance = instanceObj.AddComponent<T>();
                    DontDestroyOnLoad(instanceObj); //保证单例不被释放

                    Debug.LogFormat("添加一个新单例 {0} !", instanceName);
                }

                return instance;
            }
        }
    }
}