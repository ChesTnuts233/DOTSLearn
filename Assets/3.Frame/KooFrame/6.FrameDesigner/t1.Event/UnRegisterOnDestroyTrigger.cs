using System;
using System.Collections.Generic;
using UnityEngine;

namespace KooFrame
{
    /// <summary>
    /// 注销的触发器
    /// </summary>
    public class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        private HashSet<IUnRegister> unRegistersHash = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister) { unRegistersHash.Add(unRegister); }

        private void OnDestroy()
        {
            foreach (var unRegister in unRegistersHash)
            {
                unRegister.UnRegister();
            }

            unRegistersHash.Clear();
        }
    }
}