using System;

namespace KooFrame
{
    /// <summary>
    /// 注销接口的实现
    /// </summary>
    public class TypeEventSystemUnRegister<T> : IUnRegister
    {
        /// <summary>
        /// 事件系统
        /// </summary>
        public ITypeEventSystem TypeEventSystem { get; set; }

        /// <summary>
        /// 注销事件
        /// </summary>
        public Action<T> OnEvent { get; set; }

        public void UnRegister()
        {
            TypeEventSystem.UnRegister(OnEvent);

            TypeEventSystem = null;
            OnEvent = null;
        }
    }
}