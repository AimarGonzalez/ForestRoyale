using Sirenix.OdinInspector;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours
{
	public class UnitSquad : MonoBehaviour
	{
		[SerializeField]
		private UnitPlacement[] _placements;

		private void Awake()
		{
			_placements = GetComponentsInChildren<UnitPlacement>();
		}

		[Button, HideInPlayMode]
		private void SpawnTemporalUnits()
		{
			foreach (UnitPlacement placement in _placements)
			{
				placement.SpawnTemporalUnit();
			}
		}
	}
}