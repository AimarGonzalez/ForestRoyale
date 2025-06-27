using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ForestRoyale.Core.Pool
{
	public class GameObjectPool
	{
		private Transform _parent;
		private IObjectResolver _vcontainer;

		[ShowInInspector, ReadOnly]
		private int _numActiveObjects = 0;
		
		[ShowInInspector, ReadOnly]
		private int _numPooledObjects = 0;
		
		private int _numInstancedObjects = 0;
		
		[ShowInInspector, ReadOnly]
		private Queue<PooledGameObject> _queue = new();

		public int NumActiveObjects => _numActiveObjects;
		public int NumTotalObjects => _numActiveObjects + _numPooledObjects;
		 

		public GameObjectPool(IObjectResolver vcontainer, Transform parent)
		{
			_vcontainer = vcontainer;
			_parent = parent;
		}

		public PooledGameObject Get(PooledGameObject prefab, Transform parent)
		{
			return Get(prefab, parent, active: true, Vector3.zero, Quaternion.identity, inWorldSpace: false);
		}

		public PooledGameObject Get(PooledGameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
		{ 
			return Get(prefab, parent, active: true, position, rotation, inWorldSpace: false);
		} 

		public PooledGameObject Get(PooledGameObject prefab, Transform parent, bool active, Vector3 position, Quaternion rotation, bool inWorldSpace)
		{
			PooledGameObject instance;
			if (_queue.Count > 0)
			{
				instance = _queue.Dequeue();
				_numPooledObjects--;
			}
			else
			{
				_numInstancedObjects++;
				instance = _vcontainer.Instantiate(prefab);
				instance.name = $"{prefab.name}-{_numInstancedObjects}";
			}

			_numActiveObjects++;

			instance.Init(prefab, this);
			
			Transform transform = instance.transform;
			transform.SetParent(parent, false);

			if (inWorldSpace)
			{
				transform.position = position;
				transform.rotation = rotation;
			}
			else
			{
				transform.localPosition = position;
				transform.localRotation = rotation;
			}

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


			instance.TriggerReturnToPool();

			instance.gameObject.SetActive(false);
			instance.transform.SetParent(_parent);

			if (_queue.Contains(instance))
			{
				Debug.LogError($"Prefab reference is already in the pool: {instance.name}");
				return;
			}

			_queue.Enqueue(instance);
			_numPooledObjects++;
		}
	}
}