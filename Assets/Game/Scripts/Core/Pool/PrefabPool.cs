using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ForestRoyale.Core.Pool
{
	public class PrefabPool
	{
		private Queue<PooledGameObject> _queue = new();

		private Transform _parent;
		private IObjectResolver _vcontainer;

		private int _numActiveObjects = 0;
		private int _numPooledObjects = 0;

		public int NumActiveObjects => _numActiveObjects;
		public int NumTotalObjects => _numActiveObjects + _numPooledObjects;

		public PrefabPool(IObjectResolver vcontainer, Transform parent)
		{
			_vcontainer = vcontainer;
			_parent = parent;
		}

		public PooledGameObject Get(PooledGameObject prefab, Transform parent, bool worldPositionStays, bool active = true)
		{
			PooledGameObject instance;
			if (_queue.Count > 0)
			{
				instance = _queue.Dequeue();
			}
			else
			{
				instance = _vcontainer.Instantiate(prefab);
			}

			_numActiveObjects++;

			instance.Init(prefab, this);
			
			instance.transform.SetParent(parent, worldPositionStays);

			instance.gameObject.SetActive(active);

			instance.OnGetFromPool();

			return instance;
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
				_numActiveObjects--;
			}

			_numPooledObjects++;

			instance.OnReturnToPool();

			instance.gameObject.SetActive(false);
			instance.transform.SetParent(_parent);

			if (_queue.Contains(instance))
			{
				Debug.LogError($"Prefab reference is already in the pool: {instance.name}");
				return;
			}

			_queue.Enqueue(instance);
		}
	}
}