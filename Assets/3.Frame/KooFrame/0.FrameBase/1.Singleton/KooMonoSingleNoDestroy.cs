using System;
using UnityEngine;

namespace KooFrame
{
    /// <summary>
    /// Mono单例
    /// 运行游戏自动放置于DontDestroy
    /// </summary>
    /// <typeparam name="T">单例类型</typeparam>
    public class KooMonoSingleNoDestroy<T> : MonoBehaviour where T : KooMonoSingleNoDestroy<T>
    {
        private static T instance;

        public static T Instance => instance;

        /// <summary>
        /// 线程锁
        /// </summary>
        private static readonly object objlock = new object();

        protected virtual void Awake()
        {
            lock (objlock)
            {
                if (instance == null)
                {
                    instance = this as T;

                    DontDestroyOnLoad(this);
                }
                if (instance != null && instance != this) // 防止Editor下的Instance已经存在，并且是自身
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}