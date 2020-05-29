using UnityEditor;
using UnityEngine;


namespace Framework.Internal.Editor
{
	[CustomPropertyDrawer(typeof(SceneField))]
	public sealed class SceneFieldDrawer : PropertyDrawer
	{
		public override void OnGUI(
			Rect position,
			SerializedProperty property,
			GUIContent label)
		{
			EditorGUI.BeginProperty(position, GUIContent.none, property);
			position = EditorGUI.PrefixLabel(
				position,
				GUIUtility.GetControlID(FocusType.Passive),
				label);

			var nameField = property.FindPropertyRelative("Name");
			var assetField = property.FindPropertyRelative("_asset");
			if (assetField != null)
			{
				EditorGUI.BeginChangeCheck();
				if (assetField.objectReferenceValue == null &&
					!string.IsNullOrEmpty(nameField.stringValue))
				{
					var assets = AssetDatabase.FindAssets(nameField.stringValue);
					for (var i = 0; i < assets.Length; i++)
					{
						var path = AssetDatabase.GUIDToAssetPath(assets[i]);
						if (path.EndsWith(".unity"))
						{
							assetField.objectReferenceValue =
								AssetDatabase.LoadAssetAtPath<SceneAsset>(path);

							break;
						}
					}

				}


				var scene = EditorGUI.ObjectField(
					position,
					assetField.objectReferenceValue,
					typeof(SceneAsset),
					false) as SceneAsset;

				if (EditorGUI.EndChangeCheck())
				{
					assetField.objectReferenceValue = scene;

					var name = scene != null
						? scene.name
						: null;

					nameField.stringValue = name;
				}
			}

			EditorGUI.EndProperty();
		}
	}
}
