using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtension 
{
	public static float O1(this float f)
		=> Mathf.Clamp01(f);
}
