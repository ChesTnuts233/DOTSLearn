//****************** 代码文件申明 ************************
//* 文件：Controller                      
//* 作者：32867
//* 创建时间：2023年09月02日 星期六 21:36
//* 描述：MVC中的Controller
//*****************************************************

using System;

namespace KooFrame.MVC
{
    public abstract class Controller
    {
        //执行事件
        public abstract void Execute(object data);

        /// <summary>
        /// 获取模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetModel<T>() where T : Model
        {
            return MVC.GetModel<T>() as T;
        }

        /// <summary>
        /// 获取视图
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetView<T>() where T : View
        {
            return MVC.GetView<T>() as T;
        }

        /// <summary>
        /// 注册模型
        /// </summary>
        /// <param name="model"></param>
        public static void RegisterModel(Model model)
        {
            MVC.RegisterModel(model);
        }

        /// <summary>
        /// 注册视图
        /// </summary>
        /// <param name="view"></param>
        public static void RegisterView(View view)
        {
            MVC.RegisterView(view);
        }

        /// <summary>
        /// 注册控制器
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="controllerType"></param>
        public static void RegisterController(string eventName, Type controllerType)
        {
            MVC.RegisterController(eventName, controllerType);
        }
    }
}