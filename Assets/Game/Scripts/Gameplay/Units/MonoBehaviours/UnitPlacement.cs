using UnityEngine;
using VContainer;
using ForestRoyale.Core.Pool;
using Sirenix.OdinInspector;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours
{
	public class UnitPlacement : MonoBehaviour
	{
		[SerializeField]
		[AssetSelector(Paths = "Assets/Game/Prefabs")]
		[AssetsOnly]
		private UnitRoot _prefab;

		[Inject]
		private GameObjectPoolService _poolService;

		private UnitRoot _instance;
		
		public UnitRoot UnitRoot => _instance;

		[Button, HideInPlayMode]
		public UnitRoot SpawnPrefab()
		{
			if (Application.isPlaying)
			{
				_instance = SpanwPoolUnit();
			}
#if UNITY_EDITOR
			else
			{
				_instance = SpawnTemporalUnit();
			}
#endif
			return _instance;
		}

#if UNITY_EDITOR
		private UnitRoot SpawnTemporalUnit()
		{
			if (transform.childCount > 0)
			{
				// we have a temporal instance already - skip
				return null;
			}
			
			if (Application.isPlaying)
			{
				Debug.LogError("UnitPlacement: SpawnTemporalUnits is not allowed in play mode", this);
				return null;
			}

			if (_prefab == null)
			{
				Debug.LogError("UnitPlacement: Prefab is not set", this);
				return null;
			}

			Debug.Log($"UnitPlacement: Creating TEMPORAL new unit {_prefab.name}", this);
			UnitRoot temporalInstance = Instantiate(_prefab, transform, worldPositionStays: false);
			temporalInstance.gameObject.hideFlags = HideFlags.DontSave;
			temporalInstance.name = $"<TEMPORAL>_{_prefab.name}";
			
			return temporalInstance;
		}
#endif

		private UnitRoot SpanwPoolUnit()
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