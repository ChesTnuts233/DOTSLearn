namespace KooFrame
{
    public interface IController : IBelongToArchitecture, ICanGetSystem, ICanGetModel, ICanGetUtility, ICanRegisterEvent, ICanSendCommand
    {
    }
}