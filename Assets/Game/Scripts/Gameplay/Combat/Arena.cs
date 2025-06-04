using ForestRoyale.Gameplay.Systems;
using ForestRoyale.Gameplay.Units;
using ForestRoyale.Gameplay.Units.MonoBehaviours;
using Game.Scripts.Gameplay.Cards.CardStats;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ForestRoyale.Gameplay.Combat
{
	[DefaultExecutionOrder(-4000)]
	public class Arena : MonoBehaviour
	{
		[SerializeField] private Transform _arena;
		[SerializeField] private Grid _grid;

		private List<UnitRoot> _roots = new List<UnitRoot>();
		private List<Unit> _towers;
		
		[Inject]
		private ArenaEvents _arenaEvents;
		
		public Transform RootTransform => _arena;
		public Grid Grid => _grid;

		private void Awake()
		{
			_arenaEvents.OnSceneUnitsInitialized += HandleSceneUnitsInitialized;
		}

		private void OnDestroy()
		{
			_arenaEvents.OnSceneUnitsInitialized -= HandleSceneUnitsInitialized;
		}
		
		private void HandleSceneUnitsInitialized()
		{
			_arena.GetComponentsInChildren(includeInactive: true, _roots);
			_towers = _roots.Where(root => root.Unit.UnitType == UnitType.ArenaTower)
				.Select(root => root.Unit).ToList();
		}

		public void ResetTowers()
		{
			foreach (var tower in _towers)
			{
				tower.Reset();
				_arenaEvents.TriggerUnitCreated(tower);
			}
		}
	}
}
