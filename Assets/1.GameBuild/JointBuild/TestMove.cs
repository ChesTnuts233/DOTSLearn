//****************** 代码文件申明 ************************
//* 文件：TestMove                                       
//* 作者：32867
//* 创建时间：2023/10/27 14:47:41 星期五
//* 描述：Nothing
//*****************************************************

using UnityEngine;

namespace GameBuild
{
    public class TestMove : MonoBehaviour
    {
        public float moveForce = 10f; // 设置移动力大小
        private Rigidbody2D rb;
        [SerializeField] private float JumpForce;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector2 force = new Vector2(horizontalInput * moveForce, 0);
            rb.AddForce(force);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector2.up * JumpForce,ForceMode2D.Impulse);
            }
        }
    }
}