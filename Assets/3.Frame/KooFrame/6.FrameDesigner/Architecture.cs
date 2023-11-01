using System;
using System.Collections.Generic;
using KooFrame.Manager;
using UnityEngine;

namespace KooFrame
{
    public interface IArchitecture
    {
        /// <summary>
        /// 注册系统层
        /// </summary>
        /// <param name="system"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterSystem<T>(T system) where T : ISystem;

        /// <summary>
        /// 注册模块层
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterModel<T>(T model) where T : IModel;

        /// <summary>
        /// 注册工具层
        /// </summary>
        /// <param name="utility"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterUtility<T>(T utility) where T : IUtility;


        T GetSystem<T>() where T : class, ISystem;

        T GetModel<T>() where T : class, IModel;

        T GetUtility<T>() where T : class, IUtility;

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        TResult SendCommand<TResult>(ICommand<TResult> command);

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="T"></typeparam>
        void SendCommand<T>(T command) where T : ICommand;


        /// <summary>
        /// 发送事件
        /// </summary>
        void SendEvent<T>() where T : new();

        /// <summary>
        /// 发送事件
        /// </summary>
        void SendEvent<T>(T e);

        /// <summary>
        /// 注册事件
        /// </summary>
        IUnRegister RegisterEvent<T>(Action<T> onEvent);

        /// <summary>
        /// 注销事件
        /// </summary>
        void UnRegisterEvent<T>(Action<T> onEvent);
    }

    /// <summary>
    /// 架构层
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        private static T architecture;

        /// <summary>
        /// system的待初始化 -哈希表
        /// </summary>
        private HashSet<ISystem> _systemHashSet = new HashSet<ISystem>();

        /// <summary>
        /// model的待初始化 -哈希表
        /// </summary>
        private HashSet<IModel> _modelHashSet = new HashSet<IModel>();

        /// <summary>
        /// 是否已经初始化完毕
        /// </summary>
        private bool _hasInited = false;

        public static Action<T> OnRegisterPatch = architecture => { };


        public static IArchitecture Interface
        {
            get
            {
                if (architecture == null)
                {
                    MakeSureArchitecture();
                }

                return architecture;
            }
        }


        static void MakeSureArchitecture()
        {
            if (architecture == null)
            {
                architecture = new T();
                architecture.Init();

                OnRegisterPatch?.Invoke(architecture);

                //遍历列表初始化所有待初始化的Model
                foreach (var architectureModel in architecture._modelHashSet)
                {
                    architectureModel.Init();
                }

                //清空初始化列表
                architecture._modelHashSet.Clear();

                //遍历列表初始化所有待初始化的System
                foreach (var architectureSystem in architecture._systemHashSet)
                {
                    architectureSystem.Init();
                }

                //清空初始化列表
                architecture._systemHashSet.Clear();

                architecture._hasInited = true;
            }
        }

        /// <summary>
        /// 架构IOC
        /// </summary>
        private IOCContainer _container = new IOCContainer();

        /// <summary>
        /// 子类注册模块用
        /// </summary>
        protected abstract void Init();

        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            model.SetArchitecture(this);
            //IOC容器注册Model
            _container.Register<TModel>(model);

            if (!_hasInited)
            {
                _modelHashSet.Add(model);
            }
            else
            {
                model.Init();
            }
        }

        /// <summary>
        /// 注册系统层
        /// </summary>
        /// <param name="system"></param>
        /// <typeparam name="TSystem"></typeparam>
        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            //让system通过接口设置自己的架构实例
            system.SetArchitecture(this);
            //IOC容器注册System
            _container.Register<TSystem>(system);
            
            if (!_hasInited) //如果架构没有初始化
            {
                _systemHashSet.Add(system); //待初始化列表添加system
            }
            else
            {
                system.Init(); //直接Init此system
            }
        }


        /// <summary>
        /// 注册工具层
        /// </summary>
        /// <param name="utility"></param>
        /// <typeparam name="TUtility"></typeparam>
        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : IUtility
        {
            _container.Register<TUtility>(utility);
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem { return _container.Get<TSystem>(); }

        public TModel GetModel<TModel>() where TModel : class, IModel { return _container.Get<TModel>(); }

        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility { return _container.Get<TUtility>(); }


        public TResult SendCommand<TResult>(ICommand<TResult> command) { return ExecuteCommand(command); }

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand { ExecuteCommand(command); }

        protected virtual TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            command.SetArchitecture(this);
            return command.Execute();
        }

        protected virtual void ExecuteCommand(ICommand command)
        {
            command.SetArchitecture(this);
            command.Execute();
        }


        private TypeEventSystem mTypeEventSystem = new TypeEventSystem();

        public void SendEvent<TEvent>() where TEvent : new() { mTypeEventSystem.Send<TEvent>(); }

        public void SendEvent<TEvent>(TEvent e) { mTypeEventSystem.Send<TEvent>(e); }

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return mTypeEventSystem.Register<TEvent>(onEvent);
        }

        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent) { mTypeEventSystem.UnRegister<TEvent>(onEvent); }
    }
}