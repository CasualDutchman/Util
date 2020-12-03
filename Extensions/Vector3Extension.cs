using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public static class Vector3Extension
	{
		public static Vector2 XY(this Vector3 v3)
			=> new Vector2(v3.x, v3.y);

		public static Vector3 XYO(this Vector3 v3, float f = 0)
			=> new Vector3(v3.x, v3.y, f);

		public static Vector2 XZ(this Vector3 v3)
			=> new Vector2(v3.x, v3.z);

		public static Vector3 SetX(this Vector3 v3, float value)
			=> new Vector3(value, v3.y, v3.z);

		public static Vector3 SetY(this Vector3 v3, float value)
			=> new Vector3(v3.x, value, v3.z);

		public static Vector3 SetZ(this Vector3 v3, float value)
			=> new Vector3(v3.x, v3.y, value);

		public static Vector3 PreDiv(this Vector3 v3, float value)
			=> new Vector3(value / v3.x, value / v3.y, value / v3.z);

		public static Vector3 Mul(this Vector3 v3a, Vector3 v3b)
			=> new Vector3(v3a.x * v3b.x, v3a.y * v3b.y, v3a.z * v3b.z);

		public static Vector3 Rotate(this Vector3 v3, Quaternion rot, Vector3 pivot)
		{
			var dir = v3 - pivot;
			return rot * dir + pivot;
		}
	}
}
