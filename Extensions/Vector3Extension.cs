using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extension
{
    public static Vector2 XY(this Vector3 v3) 
		=> new Vector2(v3.x, v3.y);

	public static Vector2 XZ(this Vector3 v3) 
		=> new Vector2(v3.x, v3.z);
}
