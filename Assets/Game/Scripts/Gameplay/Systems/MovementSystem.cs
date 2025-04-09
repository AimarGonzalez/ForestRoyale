using System.Collections.Generic;
using ForestRoyale.Gameplay.Units;
using System;
using System.Diagnostics;

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
				switch (troop.State)
				{
					case UnitState.Moving:
						if (!troop.CanMove)
						{
							break;
						}
						
						if (troop.HasTarget)
						{
							troop.MovementController.MoveToTarget();
						}
						else
						{
							troop.MovementController.Stop();
						}

						break;

					case UnitState.Attacking:
						troop.MovementController.Stop();
						break;

					default:
						UnityEngine.Debug.LogError($"Unknown unit state: {troop.State}");
						break;
				}
			}
		}

	}
}