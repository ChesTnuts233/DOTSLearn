using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace KooFrame.FrameTools
{
    public static class KooRandomTool
    {
        #region Random静态拓展
        /// <summary>
        /// 从给定的一组值中随机选择一个值并返回。
        /// </summary>
        /// <typeparam name="T">值的类型。</typeparam>
        /// <param name="values">可选的值集合。</param>
        /// <returns>随机选择的值。</returns>
        public static T GetRandomValueFrom<T>(this T[] values)
        {
            // 如果没有传递任何值，返回默认值
            if (values.Length == 0)
            {
                return default(T);
            }

            // 生成一个随机索引
            int randomIndex = Random.Range(0, values.Length);

            // 返回随机索引对应的值
            return values[randomIndex];
        }
        
        /// <summary>
        /// 从给定的集合中随机选择一个值并返回。
        /// </summary>
        /// <typeparam name="T">值的类型。</typeparam>
        /// <param name="collection">可选的集合。</param>
        /// <returns>随机选择的值。</returns>
        public static T GetRandomValueFrom<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            // 将集合转换为数组，以便获取元素个数和随机索引
            T[] values = collection as T[] ?? collection.ToArray();

            if (values.Length == 0)
            {
                return default(T);
            }

            int randomIndex = UnityEngine.Random.Range(0, values.Length);

            return values[randomIndex];
        }

        #endregion
        
    }
}