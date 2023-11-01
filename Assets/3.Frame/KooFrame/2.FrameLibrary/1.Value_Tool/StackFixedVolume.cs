//****************** 代码文件申明 ************************
//* 文件：StackFixedVolume                      
//* 作者：32867
//* 创建时间：2023年08月28日 星期一 15:07
//* 描述：固定容量的栈结构
//*****************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace KooFrame
{
    /// <summary>
    /// 固定容量的栈结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class StackFixedVolume<T> : Stack<T>
    {
        [SerializeField] private int maxVolume;

        private event Action<T> OnVolumeToMax; //当容量到达最大时候

        /// <summary>
        /// 构造栈，并规定最大容量
        /// </summary>
        /// <param name="maxVolume"></param>
        public StackFixedVolume(int maxVolume)
        {
            this.MaxVolume = maxVolume;
        }

        public int MaxVolume
        {
            get => maxVolume;
            set => maxVolume = value;
        }

        public new void Push(T item)
        {
            if (base.Count == MaxVolume)
            {
                OnVolumeToMax?.Invoke(item);
                return;
            }

            base.Push(item);
        }
    }
}