using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public static class StringExtension
	{
		public static bool Empty(this string s)
			=> string.IsNullOrEmpty(s);
	}
}
