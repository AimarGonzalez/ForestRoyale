using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ForestLib.Utils.Pool
{
	[DisallowMultipleComponent]
	public class PooledGameObject : MonoBehaviour, IDisposable
	{
		[SerializeField]
		[Tooltip("If true, will log an error if the pool is not set")]
		private bool _logMissingPool = true;

		[ShowInInspector, ReadOnly]
		private PooledGameObject _prefab;

		[ShowInInspector, ReadOnly]
		private PrefabPool _pool;

		public PooledGameObject Prefab => _prefab;

		public bool CreatedOnPool { get; private set; }

		private IPooledComponent[] _subComponents;

		public void Init(PooledGameObject prefab, PrefabPool pool)
		{
			_prefab = prefab;
			_pool = pool;

			CreatedOnPool = true;
		}

		protected virtual void Awake()
		{
			_subComponents = GetComponentsInChildren<IPooledComponent>();
		}

		public virtual void OnGetFromPool()
		{
			foreach (IPooledComponent component in _subComponents)
			{
				component.OnGetFromPool();
			}
		}

		public virtual void OnReturnToPool()
		{
			foreach (IPooledComponent component in _subComponents)
			{
				component.OnReturnToPool();
			}
		}

		public virtual void OnDestroyElement()
		{
			foreach (IPooledComponent component in _subComponents)
			{
				component.OnDestroyFromPool();
			}
		}

		public void ReleaseToPool()
		{
			if (_pool == null)
			{
				if (_logMissingPool)
				{
					Debug.LogError($"Pool is not set for {gameObject.name}");
				}

				return;
			}

			_pool.Release(this);
		}

		void IDisposable.Dispose()
		{
			ReleaseToPool();
		}

	}
}