//****************** 代码文件申明 ************************
//* 文件：StringToHash                      
//* 作者：32867
//* 创建时间：2023年09月15日 星期五 18:13
//* 描述：提供自定义的String值转换为Hash值
//*****************************************************

using System.Security.Cryptography;
using System.Text;

namespace KooFrame
{
    public class StringToHash
    {
        public static string GetHash(string input)
        {
            // 创建一个SHA-256哈希算法的实例
            using (SHA256 sha256 = SHA256.Create())
            {
                // 将输入字符串转换为字节数组
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // 计算哈希值
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // 将哈希值转换为十六进制字符串表示
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2")); // 使用两位小写十六进制表示每个字节
                }

                return builder.ToString();
            }
        }
    }
}