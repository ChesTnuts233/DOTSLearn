//****************** 代码文件申明 ************************
//* 文件：DefaultScripts                                       
//* 作者：#AUTHORNAME#
//* 创建时间：#CREATETIME#
//* 描述：吸引力
//*****************************************************

using KooFrame;
using KooFrame.FrameTools;
using UnityEngine;

namespace GameBuild
{
    /// <summary>
    /// 吸引力控制
    /// </summary>
    public class GravitateController : MonoBehaviour, IController
    {
        /// <summary>
        /// 吸引方
        /// </summary>
        [SerializeField]
        public Transform AttractiveSide;


        /// <summary>
        /// 刚体
        /// </summary>
        [SerializeField, HideIfNull]
        private Rigidbody2D rb2D;


        public float attractionStrength = 1.0f; // 吸引力的强度


        [SerializeField]
        public float maxSpeed;

        public bool isAddForce = true;


        /// <summary>
        /// 是否被吸引
        /// </summary>
        [SerializeField]
        private bool isAttract;


        /// <summary>
        /// 派别
        /// </summary>
        public BubbleType BubbleType;

        /// <summary>
        /// 刚体
        /// </summary>
        public Rigidbody2D Rb2D
        {
            get => rb2D;
            set => rb2D = value;
        }

        /// <summary>
        /// 是否被吸引
        /// </summary>
        public bool IsAttract
        {
            get => isAttract;
            set => isAttract = value;
        }


        private void FixedUpdate()
        {
            LerpToSide();
        }


        private void LerpToSide()
        {
            if (!IsAttract)
                return;
            Vector2 lastVelocity = Vector2.zero;

            // //计算吸引力大小
            // float attractionForce = attractionStrength / attractionDirection.sqrMagnitude;
            AddForceTo();
            lastVelocity = Rb2D.velocity;
            isAddForce = !Rb2D.velocity.GetAbs().GreaterThan(new Vector2(maxSpeed, maxSpeed), false);
            Rb2D.velocity = lastVelocity;
        }

        public void AddForceTo()
        {
            if (!isAddForce)
                return;

            //计算两个物体之间的吸引力方向
            var transform1 = transform;
            var position = transform1.position;
            Vector2 attractionDirection = AttractiveSide.position - position;

            //吸引到吸引方
            this.Rb2D.AddForce(attractionDirection.normalized * attractionStrength);
        }


        public IArchitecture GetArchitecture()
        {
            return DoubleBubbleApp.Interface;
        }
    }
}