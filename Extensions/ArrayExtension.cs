using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public static class ArrayExtension
	{
		public static T GetRandom<T>(this T[] arr)
		{
			return arr[Random.Range(0, arr.Length)];
		}

		public static T GetRandom<T>(this T[] arr, RNG rng)
		{
			return arr[Mathf.Clamp((int)(rng.Next() * arr.Length), 0, arr.Length)];
		}
	}
}
