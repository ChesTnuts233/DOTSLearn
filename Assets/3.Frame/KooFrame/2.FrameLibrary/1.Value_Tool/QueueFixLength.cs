using System.Collections.Generic;


namespace KooFrame
{
	/// <summary>
	/// 表示对象的先进先出集合并固定长度
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class QueueFixLength<T> : Queue<T>
	{
		int length;

		/// <summary>
		/// 初始化类的新实例，并指定长度
		/// </summary>
		/// <param name="length">长度</param>
		public QueueFixLength(int length) : base(length)
		{
			this.length = length;
		}

		/// <summary>
		/// 将对象添加到结尾处
		/// </summary>
		/// <param name="item">要添加的对象</param>
		public new void Enqueue(T item)
		{
			if (base.Count == length)
				base.Dequeue();

			base.Enqueue(item);
		}

	}
}
