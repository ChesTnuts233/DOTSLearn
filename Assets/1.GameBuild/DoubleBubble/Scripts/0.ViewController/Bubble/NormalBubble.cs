using KooFrame;
using KooFrame.FrameTools;
using UnityEngine;

namespace GameBuild
{
    public class NormalBubble : BubbleBase
    {
        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            other.transform.TryGetComponent(out GravitateController bubble);

            //变小的控制
            if (other.transform.CompareTag("Bubble"))
            {
                if (bubbleType != bubble.BubbleType) //碰撞的对方跟自己不是一边的
                {
                    //改变自身尺寸
                    ChangeSize(0.2f);

                    //如果尺寸小于0.3 回收
                    if (this.transform.localScale.LessThan(new Vector3(0.3f, 0.3f, 0.3f), false))
                    {
                        GameObjectPool.Instance.StoreObject(this.gameObject);
                    }

                    //如果对方小于0.3 回收对方
                    if (other.transform.localScale.LessThan(new Vector3(0.3f, 0.3f, 0.3f), false))
                    {
                        GameObjectPool.Instance.StoreObject(other.gameObject);
                    }
                }
            }
        }

        /// <summary>
        /// 改变尺寸
        /// </summary>
        private void ChangeSize(float size)
        {
            this.transform.localScale -= Vector3.one * size;
        }
    }
}