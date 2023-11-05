using System;
using System.Collections;
using KooFrame;
using UnityEngine;

namespace GameBuild
{
    /// <summary>
    /// 冲刺泡泡
    /// </summary>
    public class RushBubble : BubbleBase
    {
        /// <summary>
        /// 引力控制
        /// </summary>
        [SerializeField]
        private GravitateController gravitateController;

        /// <summary>
        /// 冲击半径
        /// </summary>
        public float ExplosionRadius;

        /// <summary>
        /// 冲击力大小
        /// </summary>
        public float ForceValue = 5f;


        private void Start()
        {
            gravitateController.attractionStrength = 10f;
        }

        private void OnEnable()
        {
            StartCoroutine(nameof(ExplosionForce));
        }

        /// <summary>
        /// 每过一定间隔 产生爆炸的冲击力
        /// </summary>
        /// <returns></returns>
        private IEnumerator ExplosionForce()
        {
            while (true)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, ExplosionRadius);
                foreach (var collider in colliders)
                {
                    if (collider.GetComponent<Rigidbody2D>() &&
                        collider.GetComponent<BubbleBase>().bubbleType != this.bubbleType)
                    {
                        var dir = (collider.transform.position - this.transform.position);
                        collider.GetComponent<Rigidbody2D>().AddForce(dir * ForceValue, ForceMode2D.Impulse);
                    }
                }

                yield return CoroutineTool.WaitForSeconds(0.5f);
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        // protected override void OnCollisionEnter2D(Collision2D other)
        // {
        //     other.transform.TryGetComponent(out GravitateController bubble);
        //     
        //     //碰撞后 根据法线方向产生冲击力
        //     if (other.transform.CompareTag("Bubble"))
        //     {
        //         if (bubbleType != bubble.BubbleType) //碰撞的对方跟自己不是一边的
        //         {
        //             
        //         }
        //     }
        //     
        //     
        // }
    }
}