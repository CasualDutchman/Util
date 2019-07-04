using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extension
{
    public static Vector3 XOY(this Vector2 v2) 
		=> new Vector3(v2.x, 0, v2.y);
}
