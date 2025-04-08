using System.Collections.Generic;
using ForestRoyale.Gameplay.Units;
using System;

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
				if (troop.Target == null)
				{
					SetTarget(troop, FindBestTarget(troop));
				}
			}
		}

		public void SetTarget(Unit troop, Unit newTarget)
		{
			troop.Target = newTarget;
			OnTargetChanged?.Invoke(troop);
		}

		private Unit FindBestTarget(Unit troop)
		{
			// Troops have a prioritized list of target types.
			// We should iterate through the list and find the all types within sight range, and get the closest one of them.
			// If no target is found, we should go the the next preferred type, and so on.
			return null;
		}
	}
}