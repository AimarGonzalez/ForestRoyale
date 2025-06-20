using ForestLib.ExtensionMethods;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ForestLib.Utils.Pool
{
	[DisallowMultipleComponent]
	public class PooledGameObject : MonoBehaviour, IDisposable
	{
		[SerializeField]
		[Tooltip("If true, will log an error if the pool is not set")]
		private bool _logMissingPool = true;

		/// <summary>
		/// We keep a serialized reference to the prefab, so we return assets from the scene into the pool.
		/// </summary>
		[SerializeField]
		[ReadOnly]
		private GameObject _automaticPrefab;
		
		[SerializeField]
		[ReadOnly]
		private PooledGameObject _automaticComponent;
		

		[ShowInInspector, ReadOnly]
		private PooledGameObject _prefab;

		[ShowInInspector, ReadOnly]
		private PrefabPool _pool;

		public PooledGameObject Prefab => CreatedOnPool ? _prefab : _automaticComponent;

		public bool CreatedOnPool { get; private set; }

		private IPooledComponent[] _subComponents;
		
#if UNITY_EDITOR
		private void OnValidate()
		{
			UpdatePrefabReferences();
		}

		[Button]
		private void UpdatePrefabReferences()
		{
			bool isOpenInPrefabStage = PrefabStageUtility.GetCurrentPrefabStage() != null;
			if (!isOpenInPrefabStage && PrefabUtility.IsPartOfPrefabInstance(this))
			{
				String prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(this);
				_automaticPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
				_automaticComponent = _automaticPrefab.GetComponent<PooledGameObject>();
				Debug.Log($"{name} - instance in scene");
			}
		}
#endif

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