namespace KooFrame
{
    /// <summary>
    /// 为了解决从属关系而实现的接口 Get的分离接口
    /// </summary>
    public interface IBelongToArchitecture
    {
        IArchitecture GetArchitecture();
    }
}