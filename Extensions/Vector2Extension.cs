﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public static class Vector2Extension
	{
		public static Vector3 XOY(this Vector2 v2)
			=> new Vector3(v2.x, 0, v2.y);

		public static Vector3 XYO(this Vector2 v2)
			=> new Vector3(v2.x, v2.y, 0);

		public static Vector3Int XYO(this Vector2Int v2)
			=> new Vector3Int(v2.x, v2.y, 0);

		public static Vector2 Clamp(this Vector2 v2, Vector2 min, Vector2 max)
			=> new Vector2(Mathf.Clamp(v2.x, min.x, max.x), Mathf.Clamp(v2.y, min.y, max.y));

		public static Vector2Int ToVector2Int(this Vector2 v2)
			=> new Vector2Int((int)v2.x, (int)v2.y);

		public static Vector2Int ToVector2IntFloor(this Vector2 v2)
			=> new Vector2Int(Mathf.FloorToInt(v2.x), Mathf.FloorToInt(v2.y));

		public static Vector2 ToVector2(this Vector2Int v2)
			=> new Vector2(v2.x, v2.y);
	}
}
