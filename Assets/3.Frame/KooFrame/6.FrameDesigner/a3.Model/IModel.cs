namespace KooFrame
{
    /// <summary>
    /// 这里的接口 实现的是Model与架构层之间的主从关系
    /// </summary>
    public interface IModel : IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility,ICanSendEvent
    {
        void Init();
    }
}