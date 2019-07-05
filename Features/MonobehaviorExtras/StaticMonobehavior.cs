using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public class StaticMonobehavior<T> : MonoBehaviour where T : MonoBehaviour
	{
		static T _instance;

		public static T instance
		{
			get
			{
				return _instance == null ? CreateInstance() : _instance;
			}
		}

		public static T CreateInstance()
		{
			var obj = new GameObject
			{
				name = typeof(T) + " instance",
			};

			_instance = obj.AddComponent<T>();
			DontDestroyOnLoad(obj);
			return _instance;
		}
	}
}
