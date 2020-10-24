using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtension
{
    public static T GetRandom<T>(this T[] arr)
	{
		return arr[Random.Range(0, arr.Length)];
	}
}
