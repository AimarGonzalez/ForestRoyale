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
		private const string GroupPool = "Pool";

		[InfoBox("There are multiple PooledGameObject components in the same game object. This is not allowed.", InfoMessageType.Error, nameof(MultipleComponentDetected))]

		[SerializeField]
		[Tooltip("If true, will log an error if the pool is not set")]
		[BoxGroup(GroupPool), PropertyOrder(DebugUI.OrderPool - 1)]
		private bool _logMissingPool = true;

		/// <summary>
		/// We keep a serialized reference to the prefab, so we return assets from the scene into the pool.
		/// </summary>
		[SerializeField]
		// [ReadOnly, ShowIn(PrefabKind.InstanceInScene)]
		[ReadOnly]
		[BoxGroup(GroupPool), PropertyOrder(DebugUI.OrderPool)]
		private GameObject _automaticPrefab;

		[SerializeField]
		// [ReadOnly, ShowIn(PrefabKind.InstanceInScene)]
		[ReadOnly]
		[BoxGroup(GroupPool), PropertyOrder(DebugUI.OrderPool)]
		private PooledGameObject _automaticComponent;


		[ShowInInspector, ReadOnly]
		[BoxGroup(GroupPool), PropertyOrder(DebugUI.OrderPool)]
		private PooledGameObject _prefab;

		[ShowInInspector, ReadOnly]
		[BoxGroup(GroupPool), PropertyOrder(DebugUI.OrderPool)]
		private PrefabPool _pool;

		public PooledGameObject Prefab => CreatedOnPool ? _prefab : _automaticComponent;

		[ShowInInspector, ReadOnly]
		[BoxGroup(GroupPool), PropertyOrder(DebugUI.OrderPool - 1)]
		public bool CreatedOnPool { get; private set; }

		private IPooledComponent[] _subComponents;

		private bool MultipleComponentDetected => gameObject.GetComponents<PooledGameObject>().Length > 1;

#if UNITY_EDITOR
		private void OnValidate()
		{
			UpdatePrefabReferences();
		}

		/// <summary>
		/// Updating the references the characters in the scene can be reused in the pool
		/// </summary>
		[Button]
		[ShowIn(PrefabKind.InstanceInScene)]
		[BoxGroup(GroupPool), PropertyOrder(DebugUI.OrderPool)]
		private void UpdatePrefabReferences()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			if (IsPrefabInstanceInScene(this))
			{
				if (PrefabUtility.IsOutermostPrefabInstanceRoot(gameObject))
				{
					_automaticPrefab = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
					_automaticComponent = PrefabUtility.GetCorrespondingObjectFromSource(this);
				}
				else
				{
					String prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(this);
					_automaticPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
					_automaticComponent = _automaticPrefab.GetComponent<PooledGameObject>();
				}
				
				/*
				Debug.Log($"{name} - instance in scene\n" +
				$"IsAnyPrefabInstanceRoot: {PrefabUtility.IsAnyPrefabInstanceRoot(gameObject)}\n" +
				$"IsOutermostPrefabInstanceRoot: {PrefabUtility.IsOutermostPrefabInstanceRoot(gameObject)}\n" +
				$"IsPartOfVariantPrefab: {PrefabUtility.IsPartOfVariantPrefab(this)}\n" +
				$"IsPartOfPrefabInstance: {PrefabUtility.IsPartOfPrefabInstance(this)}\n" +
				$"IsPartOfPrefabAsset: {PrefabUtility.IsPartOfPrefabAsset(this)}\n" +
				$"IsPartOfAnyPrefab: {PrefabUtility.IsPartOfAnyPrefab(this)}\n");
				*/
			}
		}
		
		private bool IsPrefabInstanceInScene(Component component)
		{
			bool isPrefabStageOpen = PrefabStageUtility.GetCurrentPrefabStage() != null;
			bool isInstance = PrefabUtility.IsPartOfPrefabInstance(component);

			// We only want to update refs on instances in the scene. 
			// However after domain reload or after a base prefab change we see many additional calls
			// to Validate on the prefabs assets. We can detect this case with isNotAsset to avoid it.
			// REMARK: gameObject.scene.IsValid(); is not a valid alternative.
			bool isNotAsset = !PrefabUtility.IsPartOfPrefabAsset(component);

			return !isPrefabStageOpen && isInstance && isNotAsset;
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

		public virtual void TriggerBeforeGetFromPool()
		{
			OnBeforeGetFromPool();
			foreach (IPooledComponent component in _subComponents)
			{
				component.OnBeforeGetFromPool();
			}
		}

		public virtual void TriggerAfterGetFromPool()
		{
			OnAfterGetFromPool();
			foreach (IPooledComponent component in _subComponents)
			{
				component.OnAfterGetFromPool();
			}
		}


		public virtual void TriggerReturnToPool()
		{
			OnReturnToPool();
			foreach (IPooledComponent component in _subComponents)
			{
				component.OnReturnToPool();
			}
		}

		public virtual void TriggerDestroyFromPool()
		{
			OnDestroyFromPool();
			foreach (IPooledComponent component in _subComponents)
			{
				component.OnDestroyFromPool();
			}
		}

		protected virtual void OnBeforeGetFromPool() { }
		protected virtual void OnAfterGetFromPool() { }
		protected virtual void OnReturnToPool() { }
		protected virtual void OnDestroyFromPool() { }

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