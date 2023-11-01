# IOC容器

IOC解决的问题：
* 单例类没有访问的限制

> 如果没有访问限制，假设有两类单例 A B 
>
> 但 B单例从层级关系来说 是比A单例要高一个层级的  (高层依赖于底层)
>
> 简单的来说 B单例 依赖于 A单例 如果这个时候 如果其他人不知道这层的依赖关系 盲目的调动A单例 导致A单例的状态发生了变动 那么依赖A单例的B单例就会出现问题 

* 基本用法

```C#
public class IOCExample : MonoBehaviour
    {
        void Start()
        {
            // 创建一个 IOC 容器
            var container = new IOCContainer();
            
            // 注册一个A管理器的实例
            container.Register(new A_Manager());
            
            // 根据类型获取A管理器的实例
            var A_Manager = container.Get<A_Manager>();
            
            //A管理器处理逻辑
            A_Manager.SomeThing();
        }

        public class A_Manager
        {
            public void SomeThing()
            {
                Debug.Log("这里是A管理器");
            }
        }
    }
```

* 为什么IOC容器可以实现类似单例的作用呢 因为 面向对象中 类 class 通常是单一不变的 这侧面的达成了单例需要的唯一性。字典这样的哈希结构也保证了这个实例的唯一性