using UnityEngine;

namespace KooFrame
{
    /// <summary>
    /// 旧输入系统 用来进行测试
    /// </summary>
    public interface ITestInput
    {
        void CheckInputDown(object key);


        void CheckInputUp(object key);
    }
}