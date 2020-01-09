using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	[Serializable]
	public class MinMax
	{
		public float Min;
		public float Max;

		public MinMax(int min, int max)
		{
			Min = min;
			Max = max;
		}

		public float Evaluate(float f)
		{
			f = Mathf.Clamp01(f);
			return Mathf.Lerp(Min, Max, f);
		}
	}
}

#if UNITY_EDITOR
namespace Framework.Internal
{
	using UnityEditor;

	[CustomPropertyDrawer(typeof(MinMax))]
	public class IngredientDrawer : PropertyDrawer
	{
		const float width = 40;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var minProp = property.FindPropertyRelative("Min");
			var maxProp = property.FindPropertyRelative("Max");

			var min = minProp.floatValue;
			var max = maxProp.floatValue;

			position = EditorGUI.PrefixLabel(position, label);

			EditorGUI.MinMaxSlider(new Rect(position.x + width + 5, position.y, position.width - width - width - 10, position.height), ref min, ref max, 0f, 1f);

			var newmin = EditorGUI.DelayedFloatField(new Rect(position.x, position.y, width, position.height), min);
			if (newmin > max)
				min = max;
			else
				min = newmin;

			var right = new GUIStyle(GUI.skin.FindStyle("label"));
			right.fontStyle = FontStyle.Normal;
			right.alignment = TextAnchor.MiddleRight;

			var newmax = EditorGUI.DelayedFloatField(new Rect(position.x + position.width - width, position.y, width, position.height), max);
			if (newmax <= min)
				max = min;
			else
				max = newmax;

			minProp.floatValue = min;
			maxProp.floatValue = max;
		}
	}
}
#endif
