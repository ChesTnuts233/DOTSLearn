namespace KooFrame
{
    public interface ISystem : IBelongToArchitecture, ICanSetArchitecture,ICanGetModel,ICanGetUtility,ICanRegisterEvent,ICanSendEvent,ICanGetSystem
    {
        void Init();
    }
    
}