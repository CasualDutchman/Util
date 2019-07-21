using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public static class Vector2Extension
	{
		public static Vector3 XOY(this Vector2 v2)
			=> new Vector3(v2.x, 0, v2.y);

		public static Vector2 Clamp(this Vector2 v2, Vector2 min, Vector2 max)
			=> new Vector2(Mathf.Clamp(v2.x, min.x, max.x), Mathf.Clamp(v2.y, min.y, max.y));
	}
}
