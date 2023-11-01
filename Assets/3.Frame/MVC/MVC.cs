//****************** 代码文件申明 ************************
//* 文件：MVC                      
//* 作者：32867
//* 创建时间：2023年09月02日 星期六 21:32
//* 描述：MVC的基本架构
//*****************************************************

using System;
using System.Collections.Generic;

namespace KooFrame.MVC
{
    public class MVC
    {
        //资源

        /// <summary>
        /// 名称对应的Model
        /// </summary>
        public static Dictionary<string, Model> Models = new();

        /// <summary>
        /// 名称对应的View
        /// </summary>
        public static Dictionary<string, View> Views = new();

        /// <summary>
        /// 事件名称对应类型
        /// </summary>
        public static Dictionary<string, Type> CommandMap = new();


        /// <summary>
        /// 注册Model
        /// </summary>
        /// <param name="model">注册的Model</param>
        public static void RegisterModel(Model model)
        {
            Models[model.Name] = model;
        }

        /// <summary>
        /// 注册View
        /// </summary>
        /// <param name="view">注册的View</param>
        public static void RegisterView(View view)
        {
            //防止重复注册
            if (Views.ContainsKey(view.Name))
            {
                Views.Remove(view.Name);
            }
        }

        /// <summary>
        /// 注册Controller
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="controllerType"></param>
        public static void RegisterController(string eventName, Type controllerType)
        {
        }

        /// <summary>
        /// 获取Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetModel<T>() where T : Model
        {
            foreach (var model in Models.Values)
            {
                if (model is T)
                {
                    return (T)model;
                }
            }

            return null;
        }

        public static T GetView<T>() where T : View
        {
            foreach (var view in Views.Values)
            {
                if (view is T)
                {
                    return (T)view;
                }
            }

            return null;
        }


        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        public static void SendEvent(string eventName, object data = null)
        {
            //controller执行
            if (CommandMap.TryGetValue(eventName, out var type))
            {
                //使用动态类型实例化控制器 更加符合MVC架构的设计原则
                Controller controller = Activator.CreateInstance(type) as Controller;
                controller?.Execute(data);
            }

            //View处理多个事件
            foreach (var view in Views.Values)
            {
                if (view.AttentionList.Contains(eventName))
                {
                    view.HandleEvent(eventName, data);
                }
            }
        }
    }
}