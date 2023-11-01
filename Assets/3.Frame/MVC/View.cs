//****************** 代码文件申明 ************************
//* 文件：View                      
//* 作者：32867
//* 创建时间：2023年09月02日 星期六 21:35
//* 描述：MVC中的View类
//*****************************************************

using System.Collections.Generic;
using UnityEngine;

namespace KooFrame.MVC
{
    public abstract class View
    {
        //名称
        public abstract string Name { get; }

        //事件关心列表
        [HideInInspector] public List<string> AttentionList = new List<string>();


        /// <summary>
        /// 处理事件
        /// </summary>
        public abstract void HandleEvent(string name, object data);

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        protected void SendEvent(string eventName, object data = null)
        {
            MVC.SendEvent(eventName, data);
        }

        /// <summary>
        /// 获取模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected Model GetModel<T>() where T : Model
        {
            return MVC.GetModel<T>();
        }


        public virtual void RegisterAttentionEvent()
        {
        }
    }
}