using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public static class ListExtension
	{
		public static void Erase<T>(this List<T> list, int index)
		{
			var last = list.Count - 1;
			list[index] = list[last];
			list.RemoveAt(last);
		}

		public static void Erase<T>(this List<T> list, T item)
		{
			var index = list.IndexOf(item);
			Erase(list, item);
		}

		public static void Swap<T>(this List<T> list, int index, int index1)
		{
			var item = list[index1];
			list[index1] = list[index];
			list[index] = item;
		}
	}
}
