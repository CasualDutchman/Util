using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public static class CasualDebug
	{
		public static readonly Color defaultColor = new Color(1, 0.9f, 0.5f);

		public static void Line(Vector3 pos1, Vector3 pos2)
		{
			Debug.DrawLine(pos1, pos2, defaultColor);
		}

		public static void Ray(Vector3 position, Vector3 direction)
		{
			Debug.DrawRay(position, direction, defaultColor);
		}

		public static void Cross(Vector3 position, float size = 0.75f)
			=> Cross(position, size, defaultColor);

		public static void Cross(Vector3 position, float size, Color color)
		{
			var half = size * 0.5f;

			Debug.DrawRay(position - new Vector3(half, 0, 0), new Vector3(size, 0, 0), color);
			Debug.DrawRay(position - new Vector3(0, half, 0), new Vector3(0, size, 0), color);
			Debug.DrawRay(position - new Vector3(0, 0, half), new Vector3(0, 0, size), color);
		}

		public static void Cross(Vector3 position, float size, Color color, float time)
		{
			var half = size * 0.5f;

			Debug.DrawRay(position - new Vector3(half, 0, 0), new Vector3(size, 0, 0), color, time);
			Debug.DrawRay(position - new Vector3(0, half, 0), new Vector3(0, size, 0), color, time);
			Debug.DrawRay(position - new Vector3(0, 0, half), new Vector3(0, 0, size), color, time);
		}

		public static void Arrows(Vector3 position, float size = 1f)
		{
			Debug.DrawRay(position, new Vector3(size, 0, 0), Color.red);
			Debug.DrawRay(position, new Vector3(0, size, 0), Color.green);
			Debug.DrawRay(position, new Vector3(0, 0, size), Color.blue);
		}

		public static void Circle(Vector3 position, float radius, int quality = 20)
			=> Circle(position, radius, quality, defaultColor);

		public static void Circle(Vector3 position, float radius, int quality, Color color)
		{
			var previous = Vector3.zero;
			for (var theta = 0.0f;
				theta <= Mathf.PI * 2.0f * (quality + 1) / quality;
				theta += Mathf.PI * 2.0f / quality)
			{
				var x = radius * Mathf.Cos(theta);
				var y = radius * Mathf.Sin(theta);

				var pos = position + Vector3.right * x + Vector3.forward * y;
				if (theta > float.Epsilon)
					Debug.DrawLine(previous, pos, color);

				previous = pos;
			}
		}

		public static void Cylinder(Vector3 position, float radius, float height, int quality = 20)
			=> Cylinder(position, radius, height, quality, defaultColor);

		public static void Cylinder(Vector3 position, float radius, float height, int quality, Color color)
		{
			var previous = Vector3.zero;
			for (var theta = 0.0f;
				theta <= Mathf.PI * 2.0f * (quality + 1) / quality;
				theta += Mathf.PI * 2.0f / quality)
			{
				var x = radius * Mathf.Cos(theta);
				var y = radius * Mathf.Sin(theta);

				var pos = position + Vector3.right * x + Vector3.forward * y;
				if (theta > float.Epsilon)
				{
					var up = new Vector3(0, height, 0);

					Debug.DrawLine(previous, pos, color);
					Debug.DrawLine(previous + up, pos + up, color);
					Debug.DrawLine(previous, previous + up, color);
				}

				previous = pos;
			}
		}
	}
}
