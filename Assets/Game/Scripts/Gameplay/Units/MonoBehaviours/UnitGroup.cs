using ForestRoyale.Core.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours
{
	[ExecuteAlways]
	public class UnitGroup : PooledGameObject
	{
		[ShowInInspector, ReadOnly]
		private UnitPlacement[] _placements;

		public UnitPlacement[] Placements => _placements;

		protected override void Awake()
		{
			base.Awake();

			_placements = GetComponentsInChildren<UnitPlacement>();
		}

		protected override void OnBeforeGetFromPool()
		{
			base.OnBeforeGetFromPool();

			SpawnPrefabs();

			if (!CreatedOnPool)
			{
				CreateUnits();
			}
		}

		protected override void OnReturnToPool()
		{
			base.OnReturnToPool();

			// The TroopCastingView is responsible of reparenting the spawned prefabs.
			Debug.Assert(!HasSpawnedPrefabLeftovers(), "Returning a UnitGroup with spawned prefabs. This will generate garbage");
		}

		private bool HasSpawnedPrefabLeftovers()
		{
			foreach (UnitPlacement placement in _placements)
			{
				if (placement.transform.childCount > 0)
				{
					return true;
				}
			}

			return false;
		}

		[Button, HideInPlayMode]
		private void SpawnPrefabs()
		{
			foreach (UnitPlacement placement in _placements)
			{
				placement.SpawnPrefab();
			}
		}

		private void CreateUnits()
		{
			foreach (UnitPlacement placement in _placements)
			{
				placement.UnitRoot.CreateUnit();
			}
		}


	}
}