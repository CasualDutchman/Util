using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public static class IByteUtilizerExtension
	{
		public static int GetByteCount<T>(this T[] arr) where T : IByteUtilizer
		{
			if (arr == null || arr.Length == 0)
				return 0;

			return arr[0].GetByteCount() + 8;
		}

		public static int GetByteSize<T>(this T[] arr) where T : IByteUtilizer
		{
			if (arr == null || arr.Length == 0)
				return 0;

			var size = 0;

			for (int i = 0; i < arr.Length; i++)
			{
				if (arr[i] != null)
					size += arr[i].GetByteCount();
			}

			return size + 8 + (arr.Length * 4);
		}
		
		public static int GetByteSize<T>(this List<T> arr) where T : IByteUtilizer
		{
			if (arr == null || arr.Count == 0)
				return 0;

			var size = 0;

			for (int i = 0; i < arr.Count; i++)
			{
				if (arr[i] != null)
					size += arr[i].GetByteCount();
			}

			return size + 8 + (arr.Count * 4);
		}

		public static int GetByteSize(this string str)
			=> str.Length * 2 + 4;
	}
}
