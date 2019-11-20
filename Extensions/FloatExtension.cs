using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtension 
{
	public static float O1(this float f)
		=> Mathf.Clamp01(f);

	public static float ThresholdDown(this float f, float threshold, float min = 0)
		=> f < threshold ? min : f;

	public static float ThresholdUp(this float f, float threshold, float max = 1)
		=> f > threshold ? max : f;
}
