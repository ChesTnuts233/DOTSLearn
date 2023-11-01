using System;
using System.Collections.Generic;

namespace KooFrame
{
    public class TypeEventSystem : ITypeEventSystem
    {
        /// <summary>
        /// 注册接口
        /// </summary>
        interface IRegistrations
        {
        }

        class Registrations<T> : IRegistrations
        {
            /// <summary>
            /// 空委托(本身可以一对多注册)
            /// </summary>
            public Action<T> OnEvent = obj => { };
        }

        class Registrations<T, T1> : IRegistrations
        {
            public Action<T, T1> OnEvent = (obj, obj1) => { };
        }


        private Dictionary<Type, IRegistrations> eventRegistrationsMap = new();

        public void Send<T>() where T : new()
        {
            var e = new T();
            Send<T>(e);
        }

        public void Send<T>(T e)
        {
            var type = typeof(T);
            IRegistrations eventRegistrations;
            if (eventRegistrationsMap.TryGetValue(type, out eventRegistrations))
            {
                (eventRegistrations as Registrations<T>)?.OnEvent.Invoke(e);
            }
        }

        public IUnRegister Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations eventRegistrations;

            //字典中不存在事件的时候 新添加
            if (!eventRegistrationsMap.TryGetValue(type, out eventRegistrations))
            {
                eventRegistrations = new Registrations<T>();
                eventRegistrationsMap.Add(type, eventRegistrations);
            }

            ((Registrations<T>)eventRegistrations).OnEvent += onEvent;

            return new TypeEventSystemUnRegister<T>()
            {
                OnEvent = onEvent,
                TypeEventSystem = this
            };
        }
        

        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations eventRegistrations;

            if (eventRegistrationsMap.TryGetValue(type, out eventRegistrations))
            {
                ((Registrations<T>)eventRegistrations).OnEvent -= onEvent;
            }
        }

        public void UnRegister<T, T1>(Action<T, T1> onEvent)
        {
            var type = typeof(T);
            IRegistrations eventRegistrations;

            if (eventRegistrationsMap.TryGetValue(type, out eventRegistrations))
            {
                ((Registrations<T,T1>)eventRegistrations).OnEvent -= onEvent;
            }
            
            var type1 = typeof(T1);
            IRegistrations eventRegistrations1;

            if (eventRegistrationsMap.TryGetValue(type1, out eventRegistrations1))
            {
                ((Registrations<T,T1>)eventRegistrations1).OnEvent -= onEvent;
            }
        }
    }
}