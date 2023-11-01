namespace KooFrame
{
    public abstract class AbstractCommand : ICommand
    {
        /// <summary>
        /// 需要互相持有的架构实例
        /// </summary>
        private IArchitecture _architecture;

        /// <summary>
        /// 显式调用接口去获取架构实例
        /// </summary>
        /// <returns></returns>
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return _architecture;
        }

        /// <summary>
        /// 显示调用接口设置架构实例，
        /// 以阉割掉继承子类中使用SetArchitecture的重写需求 
        /// </summary>
        /// <param name="architecture"></param>
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }
        
        /// <summary>
        /// 显式调用接口 
        /// </summary>
        void ICommand.Execute()
        {
            OnExecute();
        }

        protected abstract void OnExecute();
    }
}