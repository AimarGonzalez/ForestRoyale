using UnityEngine;
using VContainer;
using ForestRoyale.Core.Pool;
using Sirenix.OdinInspector;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours
{
	[ExecuteAlways]
	public class UnitPlacement : MonoBehaviour
	{
		[SerializeField]
		[AssetSelector(Paths = "Assets/Game/Prefabs")]
		[AssetsOnly]
		private UnitRoot _prefab;

		[Inject]
		private GameObjectPoolService _poolService;

		private void Awake()
		{
#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				SpawnTemporalUnit();
			}
#endif
		}

#if UNITY_EDITOR
		[Button, HideInPlayMode]
		public void SpawnTemporalUnit()
		{
			if (transform.childCount > 0)
			{
				// we have a temporal instance already - skip
				return;
			}
			
			if (Application.isPlaying)
			{
				Debug.LogError("UnitPlacement: SpawnTemporalUnits is not allowed in play mode", this);
				return;
			}

			if (_prefab == null)
			{
				Debug.LogError("UnitPlacement: Prefab is not set", this);
				return;
			}

			Debug.Log($"UnitPlacement: Creating TEMPORAL new unit {_prefab.name}", this);
			UnitRoot temporalInstance = Instantiate(_prefab, transform, worldPositionStays: false);
			temporalInstance.gameObject.hideFlags = HideFlags.DontSave;
			temporalInstance.name = $"<TEMPORAL>_{_prefab.name}";
		}
#endif

		public UnitRoot SpawnUnit()
		{
			if (!Application.isPlaying)
			{
				Debug.LogError("UnitPlacement: SpawnUnit is not allowed in edit mode", this);
				return null;
			}

			if (_prefab == null)
			{
				Debug.LogError("UnitPlacement: Prefab is not set", this);
				return null;
			}

			Debug.Assert(transform.childCount == 0, "UnitPlacement: Prefab has unexpectedchildren");
			Debug.Log($"UnitPlacement: Creating new unit {_prefab.name}", this);
			return _poolService.Get(_prefab, transform);
		}

	}
}