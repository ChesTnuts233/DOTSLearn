using System;

namespace KooFrame
{
    /// <summary>
    /// 只读的BindableProperty
    /// 剔除Set功能
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadonlyBindableProperty<T>
    {
        T Value { get; }

        IUnRegister RegisterWithInitValue(Action<T> action);
        void UnRegister(Action<T> onValueChanged);
        IUnRegister Register(Action<T> onValueChanged);
    }
    
    
    public interface IBindableProperty<T> : IReadonlyBindableProperty<T>
    {
        new T Value { get; set; }
        void SetValueWithoutEvent(T newValue);
    }
}