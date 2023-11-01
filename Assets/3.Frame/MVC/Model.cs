//****************** 代码文件申明 ************************
//* 文件：Model                      
//* 作者：32867
//* 创建时间：2023年09月02日 星期六 21:32
//* 描述：MVC中的Model类
//*****************************************************

namespace KooFrame.MVC
{
    public abstract class Model
    {
        //标识
        public abstract string Name { get; }

        //发送事件
        protected void SendEvent(string eventName, object data = null)
        {
            MVC.SendEvent(eventName,data);
        }
    }
}