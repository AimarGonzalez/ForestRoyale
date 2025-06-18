using UnityEditor;
using UnityEngine;

namespace ForestLib.Utils
{
	public class PrefabInfo : MonoBehaviour, ISerializationCallbackReceiver
	{
		[SerializeField] private string _guid;
		[SerializeField] private string _prefabName;
		[SerializeField] private string _prefabPath;
		[SerializeField] private GameObject _prefab;

		public string GUID => _guid;
		public string PrefabName => _prefabName;
		public string PrefabPath => _prefabPath;
		public GameObject Prefab => _prefab;

#if UNITY_EDITOR
		public void Init()
		{
			GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
			if (prefab == null)
			{
				Debug.LogError($"the object is not a prefab! - {gameObject.name}");
				return;
			}

			_prefabName = prefab.name;
			_prefab = prefab;
			_prefabPath = AssetDatabase.GetAssetPath(prefab);
			_guid = AssetDatabase.AssetPathToGUID(_prefabPath);
		}
#endif

		public void OnBeforeSerialize()
		{
#if UNITY_EDITOR
			Init();
#endif
		}

		public void OnAfterDeserialize()
		{
#if UNITY_EDITOR
			Init();
#endif
		}
	}
}