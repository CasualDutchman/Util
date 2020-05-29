using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;


namespace Framework
{
	[Serializable]
	public struct SceneField
	{
		public string Name;

#		if UNITY_EDITOR
		[SerializeField]
		Object _asset;
#		endif

		public Scene Find()
		{
			return SceneManager.GetSceneByName(Name);
		}

		public void Load()
		{
			SceneManager.LoadScene(Name);
		}

		public AsyncOperation LoadAsync()
		{
			return SceneManager.LoadSceneAsync(Name);
		}

		public static implicit operator string(SceneField sceneField)
		{
			return sceneField.Name;
		}
	}
}
