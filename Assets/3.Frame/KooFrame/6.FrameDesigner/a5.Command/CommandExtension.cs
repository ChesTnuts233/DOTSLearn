namespace KooFrame
{
    public static class CanSendCommandExtension
    {
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>(new T());
        }

        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand
        {
            self.GetArchitecture().SendCommand<T>(command);
        }

        public static TResult SendCommand<TResult>(this ICanSendCommand self, ICommand<TResult> command)
        {
            return self.GetArchitecture().SendCommand(command);
        }
    }
}