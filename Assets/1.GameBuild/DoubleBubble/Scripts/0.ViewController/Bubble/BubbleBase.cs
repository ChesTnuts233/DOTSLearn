using KooFrame;
using KooFrame.FrameTools;
using Sirenix.Utilities;
using UnityEngine;

namespace GameBuild
{
    public class BubbleBase : MonoBehaviour, IController
    {
        /// <summary>
        /// 泡泡的碰撞体
        /// </summary>
        [SerializeField, HideIfNull]
        private Collider2D _collider2D;

        /// <summary>
        /// 派别
        /// </summary>
        public BubbleType bubbleType;


        

        

        public IArchitecture GetArchitecture()
        {
            return DoubleBubbleApp.Interface;
        }
    }
}