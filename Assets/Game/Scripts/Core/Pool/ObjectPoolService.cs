using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Core.Pool
{
	public class ObjectPoolService : MonoBehaviour
	{
		public class Handler : MonoBehaviour, IPoolHandler
		{
			[ShowInInspector] private PooledGameObject _prefabInfo;
			[ShowInInspector] private ObjectPoolService _poolService;

			public void Init(PooledGameObject prefabInfo, ObjectPoolService poolService)
			{
				_prefabInfo = prefabInfo;
				_poolService = poolService;
			}

			public void ReturnToPool()
			{
				_poolService.Release(_prefabInfo);
			}
		}

		private Dictionary<PooledGameObject, PrefabPool> _pools = new();

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

			PrefabPool pool = GetOrCreatePool(prefab);
			return pool.Get(prefab, parent, worldPositionStays, active);
		}


		public void Release(GameObject instance)
		{
			if (instance == null)
			{
				Debug.LogError($"Returned instance is null");
				return;
			}

			PooledGameObject prefabInfo = instance.GetComponent<PooledGameObject>();
			Release(prefabInfo);
		}

		public void Release(PooledGameObject instance)
		{
			if (instance == null)
			{
				Debug.LogError($"Returned instance is null");
				return;
			}

			if (instance.CreatedOnPool)
			{
				instance.ReleaseToPool();
			}
			else
			{
				PrefabPool pool = GetOrCreatePool(instance.Prefab);
				pool.Release(instance);
			}

			
		}

		private PrefabPool GetOrCreatePool(PooledGameObject prefab)
		{
			if (!_pools.TryGetValue(prefab, out PrefabPool pool))
			{
				pool = new PrefabPool(transform);
				_pools[prefab] = pool;
			}
			return pool;
			
		}

		private void OnGUI()
		{
			GUIUtils.PushFontSize(40);
			
			// area centered on the middle right of the screen
			GUILayout.BeginArea(new Rect(Screen.width - 200, Screen.width*0.5f - 200f, 200, 500), GUI.skin.box);

			GUILayout.BeginVertical();
			foreach (var pool in _pools)
			{
				GUILayout.Label($"{pool.Key.name}: {pool.Value.NumActiveObjects} / {pool.Value.NumTotalObjects}");
			}
			GUILayout.EndVertical();

			GUILayout.EndArea();
			
			GUIUtils.PopFontSize();
		}
	}
}