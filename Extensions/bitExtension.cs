using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public static class bitExtension
	{
		public static bool Has(this byte b, byte other)
			=> (b & other) > 0;

		public static bool Has(this byte b, byte other, params byte[] more)
		{
			var o = b & other;
			for (int i = 0; i < more.Length; i++)
				o |= b & more[i];
			return o > 0;
		}

		public static bool Has(this short s, short other)
			=> (s & other) > 0;

		public static bool Has(this short s, short other, params short[] more)
		{
			var o = s & other;
			for (int i = 0; i < more.Length; i++)
				o |= s & more[i];
			return o > 0;
		}

		public static bool Has(this int i, int other)
			=> (i & other) > 0;

		public static bool Has(this int i, int other, params int[] more)
		{
			var o = i & other;
			for (int j = 0; j < more.Length; j++)
				o |= i & more[j];
			return o > 0;
		}
	}
}