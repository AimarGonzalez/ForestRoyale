using UnityEngine;
using VContainer;
using ForestRoyale.Core.Pool;
using System;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours
{
	public class UnitPlacement : MonoBehaviour
	{
		[SerializeField]
		[ExecuteAlways]
		private UnitRoot _prefab;

		[Inject]
		private GameObjectPoolService _poolService;

		private void Awake()
		{
#if UNITY_EDITOR
			SpawnTemporalUnit();
#endif
		}

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
			return _poolService.Get(_prefab, transform, worldPositionStays: false);
		}

#if UNITY_EDITOR
		public void SpawnTemporalUnit()
		{
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

			Debug.Log($"UnitPlacement: Creating new unit {_prefab.name}", this);
			UnitRoot instance = Instantiate(_prefab, transform, worldPositionStays: false);
			instance.hideFlags = HideFlags.DontSave;
		}
#endif
	}
}