using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ForestRoyale.Core.Pool
{
	public class PrefabPool
	{
		private Transform _parent;
		private IObjectResolver _vcontainer;

		[ShowInInspector, ReadOnly]
		private int _numActiveObjects = 0;
		
		[ShowInInspector, ReadOnly]
		private int _numPooledObjects = 0;
		
		[ShowInInspector, ReadOnly]
		private Queue<PooledGameObject> _queue = new();

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

			instance.TriggerBeforeGetFromPool(); // UnitRoot.CreateUnit
			instance.gameObject.SetActive(active);
			instance.TriggerAfterGetFromPool();

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

			instance.TriggerReturnToPool();

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