using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public static class Vector4Extension
	{
		public static Vector4 XYZO(this Vector3 v3, float f = 0)
			=> new Vector4(v3.x, v3.y, v3.z, f);
	}
}
