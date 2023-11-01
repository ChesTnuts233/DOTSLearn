using System.Collections.Generic;
using UnityEngine;

namespace KooFrame.Module
{
    
    public class SimpleFSM
    {
        //当前状态
        private SimpleFSMState curveState;
        
        // /// <summary>
        // /// 当前状态的状态ID
        // /// </summary>
        // private string currentStateID;
 
        /// <summary>
        /// 存储状态
        /// </summary>
        private Dictionary<string, SimpleFSMState> statesDic = new Dictionary<string, SimpleFSMState>();

        /// <summary>
        /// 存储事件对应的跳转
        /// </summary>
        public Dictionary<string, SimpleFSMTranslation> TranslationsDic = new Dictionary<string, SimpleFSMTranslation>();

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="state">添加的状态</param>
        public void AddState(SimpleFSMState state)
        {
            statesDic[state.Name] = state;
        }

        /// <summary>
        /// 添加跳转
        /// </summary>
        public void AddTranslation(SimpleFSMTranslation translation)
        {
            TranslationsDic[translation.name] = translation;
        }

        /// <summary>
        /// 启动状态机
        /// </summary>
        /// <param name="state"></param>
        public void StartFSM(SimpleFSMState state)
        {
            curveState = state;
        }

        public void HandleEvent(string name)
        {
            if (curveState != null && TranslationsDic.ContainsKey(name))
            {
                Debug.Log("FromState" + curveState.Name);

                TranslationsDic[name].callFunc();
                //当前状态转换为目标状态
                curveState = TranslationsDic[name].ToState;
                Debug.Log("toState" + curveState.Name);
            }
        }
    }
}