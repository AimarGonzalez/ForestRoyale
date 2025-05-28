using ForestLib.Gameplay.Units;
using System.Collections.Generic;
using ForestRoyale.Gameplay.Units;
using Game.Scripts.Gameplay.Cards.CardStats;
using System;
using System.Linq;
using NUnit.Framework;

namespace ForestRoyale.Gameplay.Systems
{
	public class TargetingSystem
	{
		public event Action<Unit> OnTargetChanged;

		private readonly ArenaEvents _arenaEvents;
		private readonly HashSet<Unit> _activeUnits;

		public TargetingSystem(ArenaEvents arenaEvents)
		{
			_arenaEvents = arenaEvents;
			_activeUnits = new HashSet<Unit>();

			_arenaEvents.OnUnitCreated += HandleUnitCreated;
			_arenaEvents.OnUnitDestroyed += HandleUnitDestroyed;
		}

		private void HandleUnitCreated(Unit unit)
		{
			_activeUnits.Add(unit);
		}

		private void HandleUnitDestroyed(Unit unit)
		{
			_activeUnits.Remove(unit);
		}

		public void UpdateTargets()
		{
			foreach (Unit troop in _activeUnits)
			{
				// If troop doesn't have a target, find a new target
				if (troop.State is UnitState.Idle or UnitState.MovingToTarget)
				{
					SetTarget(troop, FindBestTarget(troop));
				}
			}
		}

		public void SetTarget(Unit troop, Unit newTarget)
		{
			if (troop.Target != newTarget)
			{
				troop.Target = newTarget;
				OnTargetChanged?.Invoke(troop);
			}
		}

		private Unit FindBestTarget(Unit troop)
		{
			Unit foundTarget = null;

			foreach (UnitType targetType in troop.CombatStats.TargetPreference)
			{
				if (FindClosestTarget(troop, targetType, out foundTarget))
				{
					return foundTarget;
				}
			}

			// If no target is found look for the closest Tower
			FindClosestTarget(troop, UnitType.ArenaTower, out foundTarget);

			return foundTarget;
		}

		private bool FindClosestTarget(Unit troop, UnitType targetType, out Unit result)
		{
			result = _activeUnits.Where(unit => unit.Team != troop.Team && unit.UnitStats.UnitType == targetType).
							OrderBy(target => troop.Position.DistanceSquared(target.Position)).
							FirstOrDefault();

			return result != null;
		}
	}
}