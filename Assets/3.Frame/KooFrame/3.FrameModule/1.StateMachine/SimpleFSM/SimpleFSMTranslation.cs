namespace KooFrame.Module
{
    //定义委托
    public delegate void FSMTranslationCallFunc();
    public class SimpleFSMTranslation
    {
        public SimpleFSMState FromState;
        public string name;
        public SimpleFSMState ToState;
        public FSMTranslationCallFunc callFunc; //回调函数

        public SimpleFSMTranslation(SimpleFSMState fromState, string name, SimpleFSMState toState, FSMTranslationCallFunc callFunc)
        {
            FromState = fromState;
            this.name = name;
            ToState = toState;
            this.callFunc = callFunc;
        }
    }
}