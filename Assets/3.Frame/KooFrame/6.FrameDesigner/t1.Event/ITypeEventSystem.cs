using System;
using KooFrame;

public interface ITypeEventSystem
{
    /// <summary>
    /// 发送事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void Send<T>() where T : new();

    void Send<T>(T e);

    /// <summary>
    /// 注册事件单参数
    /// </summary>
    /// <param name="onEvent">事件发生时的回调</param>
    IUnRegister Register<T>(Action<T> onEvent);
    


    /// <summary>
    /// 注销事件
    /// </summary>
    void UnRegister<T>(Action<T> onEvent);

  
}