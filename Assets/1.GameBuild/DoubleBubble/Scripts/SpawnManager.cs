//****************** 代码文件申明 ************************
//* 文件：SpawnManager                                       
//* 作者：#AUTHORNAME#
//* 创建时间：#CREATETIME#
//* 描述：Nothing
//*****************************************************

using UnityEngine;
using Sirenix.OdinInspector;

namespace GameBuild
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField]
        private SpawnBubble Red;

        [SerializeField]
        private SpawnBubble Blue;


        [Button]
        private void TestSend(int num,ShootType type)
        {
            Red.ShootBubble(num,type);
            Blue.ShootBubble(num,type);
        }
    }
}