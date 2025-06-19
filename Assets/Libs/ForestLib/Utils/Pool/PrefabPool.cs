using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace ForestLib.Utils.Pool
{
	public class PrefabPool : MonoBehaviour
	{
		public class Handler : MonoBehaviour, IPoolHandler
		{
			[ShowInInspector] private PooledGameObject _prefabInfo;
			[ShowInInspector] private PrefabPool _pool;

			public void Init(PooledGameObject prefabInfo, PrefabPool pool)
			{
				_prefabInfo = prefabInfo;
				_pool = pool;
			}

			public void ReturnToPool()
			{
				_pool.Return(_prefabInfo);
			}
		}

		private Dictionary<PooledGameObject, Queue<PooledGameObject>> _pools = new();
		private int _numActiveObjects = 0;
		private int _numPooledObjects = 0;

		public T Get<T>(T prefab, Transform parent = null, bool worldPositionStays = true, bool active = true) where T : MonoBehaviour
		{
			PooledGameObject pooledGameObject = prefab.GetComponent<PooledGameObject>();
			PooledGameObject instance = Get(pooledGameObject, parent, worldPositionStays, active);
			return instance.GetComponent<T>();
		}


		public PooledGameObject Get(PooledGameObject prefab, Transform parent = null, bool worldPositionStays = true, bool active = true)
		{
			if (prefab == null)
			{
				Debug.LogError($"Prefab reference is null");
				return null;
			}

			// Instance from pool
			PooledGameObject instance = null;
			if (!_pools.TryGetValue(prefab, out var pool))
			{
				pool = new Queue<PooledGameObject>();
				_pools[prefab] = pool;
			}
			else if (pool.Count > 0)
			{
				instance = pool.Dequeue();
			}

			if (instance == null)
			{
				instance = Instantiate(prefab);
			}

			_numActiveObjects++;

			instance.Init(prefab, this);

			instance.transform.SetParent(parent, worldPositionStays);

			instance.gameObject.SetActive(active);

			instance.OnGetFromPool();

			return instance;
		}


		public void Return(GameObject instance)
		{
			if (instance == null)
			{
				Debug.LogError($"Returned instance is null");
				return;
			}

			PooledGameObject prefabInfo = instance.GetComponent<PooledGameObject>();
			Return(prefabInfo);
		}

		public void Return(PooledGameObject instance)
		{
			if (instance == null)
			{
				Debug.LogError($"Returned instance is null");
				return;
			}

			if (instance.CreatedOnPool)
			{
				_numActiveObjects--;
			}

			_numPooledObjects++;

			instance.OnReturnToPool();

			instance.gameObject.SetActive(false);
			instance.transform.SetParent(transform);

			if (!_pools.TryGetValue(instance.Prefab, out var queue))
			{
				queue = new Queue<PooledGameObject>();
				_pools[instance.Prefab] = queue;
			}
			else if (queue.Contains(instance))
			{
				Debug.LogError($"Prefab reference is already in the pool: {instance.Prefab.name}");
				return;
			}

			queue.Enqueue(instance);
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