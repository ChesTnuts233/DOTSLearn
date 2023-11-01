namespace KooFrame
{
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetSystem, ICanGetUtility,ICanSendEvent, ICanSendCommand
    {
        void Execute();
    }

    public interface ICommand<TResult> : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetModel,
        ICanGetUtility, ICanSendCommand
    {
        TResult Execute();
    }
}