namespace KooFrame
{
    /// <summary>
    /// 提供的单例类
    /// </summary>
    /// <typeparam name="T">单例的类型</typeparam>
    public abstract class KooSingleton<T> where T : KooSingleton<T>, new()
    {
        private static T instance; //单例实例

        private static readonly object objlock = new object(); //线程锁

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                lock (objlock)
                {
                    instance ??= new T();
                }

                return instance;
            }
        }

        //没有人会去new单例 所以不需要构造函数
        // private KooSingleton()
        // {
        //     
        // }
    }
}