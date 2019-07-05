using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public static class Vector3Extension
	{
		public static Vector2 XY(this Vector3 v3)
			=> new Vector2(v3.x, v3.y);

		public static Vector2 XZ(this Vector3 v3)
			=> new Vector2(v3.x, v3.z);

		public static Vector3 SetX(this Vector3 v3, float value)
			=> new Vector3(value, v3.y, v3.z);

		public static Vector3 SetY(this Vector3 v3, float value)
			=> new Vector3(v3.x, value, v3.z);

		public static Vector3 SetZ(this Vector3 v3, float value)
			=> new Vector3(v3.x, v3.y, value);
	}
}
