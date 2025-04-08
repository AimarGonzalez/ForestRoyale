using System.Collections.Generic;
using ForestRoyale.Gameplay.Units;
using System;

namespace ForestRoyale.Gameplay.Systems
{
	public class MovementSystem
	{
		private readonly ArenaEvents _arenaEvents;
		private readonly HashSet<Unit> _activeUnits;

		public MovementSystem(ArenaEvents arenaEvents)
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

		public void UpdateMovement()
		{
			foreach (Unit troop in _activeUnits)
			{
				if (troop.HasTarget)
				{
					if (troop.TargetIsInCombatRange)
					{
						//TODO Attack
					}
					else
					{
						troop.MovementController.MoveToTarget();
					}
				}
			}
		}

	}
}