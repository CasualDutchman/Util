using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;
using System;

namespace Framework
{
	public class EditorHelper
	{
		public static void MinMaxSlider(SerializedProperty prop, float min = 0f, float max = 1f)
		{
			var minmax = prop.vector2Value;
			prop.vector2Value = MinMaxSlider(prop.name, minmax, min, max);
		}

		public static Vector2 MinMaxSlider(string name, Vector2 v2, float min = 0f, float max = 1f)
		{
			var x = v2.x;
			var y = v2.y;
			EditorGUILayout.MinMaxSlider(name, ref x, ref y, min, max);

			var nextRect = EditorGUILayout.GetControlRect();
			nextRect = EditorGUI.PrefixLabel(nextRect, new GUIContent(" "));
			x = EditorGUI.FloatField(new Rect(nextRect.x, nextRect.y, 60, nextRect.height), x);
			y = EditorGUI.FloatField(new Rect(nextRect.x + nextRect.width - 60, nextRect.y, 60, nextRect.height), y);

			return new Vector2(x, y);
		}

		public static void Seperator(string text, float verticalMargin = 10f, float labelMargin = 7f)
		{
			var content = new GUIContent(text);

			GUILayout.Space(verticalMargin);
			EditorGUILayout.BeginHorizontal();
			{
				var firstRect = EditorGUILayout.GetControlRect(false, 1f);
				firstRect.y = firstRect.y + labelMargin;
				EditorGUI.DrawRect(firstRect, Color.grey);

				var centered = GUI.skin.GetStyle("Label");
				centered.alignment = TextAnchor.MiddleCenter;
				centered.fontStyle = FontStyle.Bold;

				EditorGUILayout.LabelField(content, centered, GUILayout.MaxWidth(centered.CalcSize(content).x + 50));

				var secondRect = EditorGUILayout.GetControlRect(false, 1f);
				secondRect.y = secondRect.y + labelMargin;
				EditorGUI.DrawRect(secondRect, Color.grey);
			}
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(verticalMargin);
		}

		public static Sprite BigSelection(Sprite sprite, string label = default)
		{
			if (!string.IsNullOrEmpty(label))
			{
				var style = new GUIStyle(GUI.skin.label);
				style.alignment = TextAnchor.UpperCenter;
				GUILayout.Label(label, style);
			}
			return (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), false, GUILayout.Width(80), GUILayout.Height(80));
		}

		public static void ShowArray<T>(ref T[] arr, Action<T> action) where T : new()
		{
			EditorGUILayout.BeginVertical("HelpBox");
			for (int i = 0; i < arr.Length; i++)
			{
				action(arr[i]);
			}

			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.MaxWidth(40)))
				{
					arr = Resize(arr, arr.Length + 1);
				}
				if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.MaxWidth(40)))
				{
					arr = Resize(arr, arr.Length - 1);
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}

		public static T[] Resize<T>(T[] arr, int amount) where T : new()
		{
			if (amount <= 0)
				return new T[0];

			var newArr = new T[amount];

			for (int i = 0; i < amount; i++)
			{
				if (i >= arr.Length)
					newArr[i] = new T();
				else
					newArr[i] = arr[i];
			}

			arr = null;
			return newArr;
		}
	}
}
#endif
