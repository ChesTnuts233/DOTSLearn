using System;
using System.Collections.Generic;
using KooFrame.FrameTools;
using UnityEditor;
using UnityEngine;

namespace KooFrame
{
    /// <summary>
    /// 通用工具集
    /// <para>这里希望放一些通用的，常用的方法</para>
    /// </summary>
    public static class KooUtils
    {
        public static void OpenLocalPath()
        {
            KooFileTool.OpenLocalPath();
        }

        public static void SetCopyBuffer(string context)
        {
            GUIUtility.systemCopyBuffer = context;
        }
        
        #region Editor工具

        private static Dictionary<string, Action<object>> myMessages = new Dictionary<string, Action<object>>();

        public static void RegisterMgs(string mgsName,Action<object> mgs)
        {
            myMessages.Add(mgsName,mgs);
        }

        public static void UnRegisterMgs(string mgsName)
        {
            myMessages.Remove(mgsName);
        }

        public static void SendMgs(string mgsName,object data)
        {
            myMessages[mgsName](data);
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// 调用MenuItem命令(有1个默认参数，默认为false)
        /// </summary>
        /// <param name="menuName">Koo框架内的有效命令</param>
        /// <param name="isCommon">是否限定于KooFramework</param>
        public static void CallMenuItem(string menuName, bool isCommon = false)
        {
            if (isCommon)
            {
                EditorApplication.ExecuteMenuItem(menuName);
            }

            EditorApplication.ExecuteMenuItem("KooFramework/" + menuName);
        }
#endif

        #endregion
    }
}  