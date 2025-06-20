using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ForestRoyale.Core.Pool
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
		[ReadOnly, ShowIn(PrefabKind.InstanceInScene)]
		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private GameObject _automaticPrefab;

		[SerializeField]
		[ReadOnly, ShowIn(PrefabKind.InstanceInScene)]
		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private PooledGameObject _automaticComponent;


		[ShowInInspector, ReadOnly]
		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private PooledGameObject _prefab;

		[ShowInInspector, ReadOnly]
		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
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
		[ShowIn(PrefabKind.InstanceInScene)]
		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private void UpdatePrefabReferences()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			bool isOpenInPrefabStage = PrefabStageUtility.GetCurrentPrefabStage() != null;
			if (!isOpenInPrefabStage && PrefabUtility.IsPartOfPrefabInstance(this) && !PrefabUtility.IsPartOfPrefabAsset(this))
			{
				String prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(this);

				if (PrefabUtility.IsOutermostPrefabInstanceRoot(gameObject))
				{
					_automaticPrefab = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
					_automaticComponent = PrefabUtility.GetCorrespondingObjectFromSource(this);
				}
				else
				{
					_automaticPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
					_automaticComponent = _automaticPrefab.GetComponent<PooledGameObject>();
				}


				Debug.Log($"{name} - instance in scene\n" +
				$"IsAnyPrefabInstanceRoot: {PrefabUtility.IsAnyPrefabInstanceRoot(gameObject)}\n" +
				$"IsOutermostPrefabInstanceRoot: {PrefabUtility.IsOutermostPrefabInstanceRoot(gameObject)}\n" +
				$"IsPartOfVariantPrefab: {PrefabUtility.IsPartOfVariantPrefab(this)}\n" +
				$"IsPartOfPrefabInstance: {PrefabUtility.IsPartOfPrefabInstance(this)}\n" +
				$"IsPartOfPrefabAsset: {PrefabUtility.IsPartOfPrefabAsset(this)}\n" +
				$"IsPartOfAnyPrefab: {PrefabUtility.IsPartOfAnyPrefab(this)}\n" +
				"");
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