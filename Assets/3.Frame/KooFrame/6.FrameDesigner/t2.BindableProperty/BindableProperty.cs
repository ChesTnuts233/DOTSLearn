using System;

namespace KooFrame
{
    /// <summary>
    /// 数据绑定属性
    /// </summary>
    /// <typeparam name="T">绑定类型</typeparam>
    public class BindableProperty<T> : IBindableProperty<T>
    {
        public BindableProperty(T defaultValue = default) { value = defaultValue; }

        /// <summary>
        /// 数值
        /// </summary>
        protected T value;

        public T Value
        {
            get => GetValue();
            set
            {
                if (value == null && this.value == null) return;
                if (value != null && value.Equals(this.value)) return;

                SetValue(value);
                _onValueChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// 改变值而触发的委托
        /// </summary>
        private Action<T> _onValueChanged = (v) => { };

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="newValue">新的赋值</param>
        protected virtual void SetValue(T newValue) { value = newValue; }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <returns>数据绑定中的值</returns>
        protected virtual T GetValue() { return value; }

        /// <summary>
        /// 设置属性值但不触发事件
        /// </summary>
        /// <param name="newValue">设置的新值</param>
        public void SetValueWithoutEvent(T newValue) { value = newValue; }


        public IUnRegister Register(Action<T> onValueChanged)
        {
            _onValueChanged += onValueChanged;
            return new BindablePropertyUnRegister<T>()
            {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }

        /// <summary>
        /// 注销某个改值而触发的委托
        /// </summary>
        /// <param name="onValueChanged"></param>
        public void UnRegister(Action<T> onValueChanged) { _onValueChanged -= onValueChanged; }

        /// <summary>
        /// 注册初始值
        /// </summary>
        /// <param name="onValueChanged">第一次设置值触发的委托</param>
        /// <returns>返回的是注销的方法接口</returns>
        public IUnRegister RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(value);
            return Register(onValueChanged);
        }

        public static implicit operator T(BindableProperty<T> property) { return property.Value; }

        public override string ToString() { return Value.ToString(); }
    }

    /// <summary>
    /// 数据绑定属性中改值委托的注销
    /// <para>这里使用的是TypeEvent 来进行注销操作</para>
    /// </summary>
    /// <typeparam name="T">属性类型</typeparam>
    public class BindablePropertyUnRegister<T> : IUnRegister
    {
        /// <summary>
        /// 数值绑定的本身
        /// </summary>
        public BindableProperty<T> BindableProperty { get; set; }

        /// <summary>
        /// 改值的委托
        /// </summary>
        public Action<T> OnValueChanged { get; set; }

        /// <summary>
        /// 注销操作
        /// </summary>
        public void UnRegister()
        {
            //注销委托
            BindableProperty.UnRegister(OnValueChanged);
            //绑定属性注销类中字段置空
            //注意 这里只是BindablePropertyUnRegister 的 BindableProperty 和 OnValueChange引用置空 本体不受影响
            BindableProperty = null;
            OnValueChanged = null;
        }
    }
}