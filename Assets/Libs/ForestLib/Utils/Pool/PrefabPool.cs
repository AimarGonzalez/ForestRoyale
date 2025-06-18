using ForestLib.Utils.Pool;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace ForestLib.Utils
{
	public class PrefabPool : MonoBehaviour
	{
		public class Handler : MonoBehaviour, IPoolHandler
		{
			[ShowInInspector] private PrefabInfo _prefabInfo;
			[ShowInInspector] private PrefabPool _pool;

			public void Init(PrefabInfo prefabInfo, PrefabPool pool)
			{
				_prefabInfo = prefabInfo;
				_pool = pool;
			}

			public void ReturnToPool()
			{
				_pool.Return(_prefabInfo);
			}
		}


		/// <summary>
		/// Key: Prefab path
		/// Value: Queue of instances
		/// </summary>
		private Dictionary<string, Queue<GameObject>> _pool = new Dictionary<string, Queue<GameObject>>();
		private List<GameObject> _activeObjects = new List<GameObject>();
		private int _numActiveObjects = 0;
		private int _numPooledObjects = 0;


		public GameObject Get(PrefabInfo prefabReference, Transform parent = null, bool worldPositionStays = true, bool active = true)
		{
			if (prefabReference == null)
			{
				Debug.LogError($"Prefab reference is null");
				return null;
			}

			// Instance from pool
			string prefabPath = prefabReference.PrefabPath;
			GameObject instance = null;
			if (!_pool.TryGetValue(prefabPath, out var queue))
			{
				queue = new Queue<GameObject>();
				_pool[prefabPath] = queue;
			}
			else if (queue.Count > 0)
			{
				instance = queue.Dequeue();
			}

			if (instance == null)
			{
				instance = Instantiate(prefabReference.Prefab);
			}

			_activeObjects.Add(instance);
			_numActiveObjects++;

			// Set handler
			if (!instance.TryGetComponent(out Handler handler))
			{
				handler = instance.AddComponent<Handler>();
				handler.Init(prefabReference, this);
			}

			// Set parent
			instance.transform.SetParent(parent, worldPositionStays);

			// Activation
			instance.SetActive(active);

			var onPoolInstances = instance.GetComponentsInChildren<IOnPoolInstance>();
			foreach (var onPoolInstance in onPoolInstances)
			{
				onPoolInstance.OnPoolInstance();
			}

			return instance;
		}


		public void Return(GameObject instance)
		{
			if (instance == null)
			{
				Debug.LogError($"Returned instance is null");
				return;
			}

			PrefabInfo prefabInfo = instance.GetComponent<PrefabInfo>();
			Return(prefabInfo);
		}

		public void Return(PrefabInfo instance)
		{
			if (instance == null)
			{
				Debug.LogError($"Returned instance is null");
				return;
			}
			var go = instance.gameObject;

			if (_activeObjects.Remove(go))
			{
				_numActiveObjects--;
			}

			_numPooledObjects++;

			var onPoolReturns = go.GetComponentsInChildren<IOnPoolReturn>();
			foreach (var onPoolReturn in onPoolReturns)
			{
				onPoolReturn.OnPoolReturn();
			}

			go.SetActive(false);
			go.transform.SetParent(transform);

			if (!_pool.TryGetValue(instance.PrefabPath, out var queue))
			{
				queue = new Queue<GameObject>();
				_pool[instance.PrefabPath] = queue;
			}
			else if (queue.Contains(go))
			{
				Debug.LogError($"Prefab reference is already in the pool: {instance.PrefabPath}");
				return;
			}

			queue.Enqueue(go);
		}

		private void OnGUI()
		{
			// area centered on the middle right of the screen
			GUILayout.BeginArea(new Rect(Screen.width - 200, 10, 200, 100), GUI.skin.box);

			GUILayout.BeginVertical();
			GUILayout.Label($"Active: {_numActiveObjects}");
			GUILayout.Label($"Pooled: {_numPooledObjects}");
			GUILayout.EndVertical();

			GUILayout.EndArea();
		}
	}
}