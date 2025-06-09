using ForestRoyale.Gameplay.UI;
using System;
using UnityEngine;

namespace ForestRoyale.Gameplay.Settings
{
	[DefaultExecutionOrder(-9999)]
	public class GameSettings : MonoBehaviour
	{
		[NonSerialized]
		private static GameSettings _instance;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ClearInstance()
		{
			if (_instance != null)
			{
				Debug.LogError("instance already initialized - this confirms NonSerialized attr is not enough");
			}

			_instance = null;
		}

		public static GameSettings Instance
		{
			get
			{
				if (_instance != null)
				{
					return _instance;
				}

				var instances = FindObjectsByType<GameSettings>(FindObjectsSortMode.None);
				if (instances.Length == 0)
				{
					Debug.LogError("No GameSettings found in scene");
					return null;
				}

				_instance = instances[0];

				return _instance;
			}
		}

		private void Awake()
		{
			if (_instance != this)
			{
				Debug.LogError("GameSettings already initialized with invalid asset references");
			}
		}

		[SerializeField]
		private UISettings _uiSettings;

		public UISettings UISettings => _uiSettings;
	}
}
