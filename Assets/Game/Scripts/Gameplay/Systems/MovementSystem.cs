using System.Collections.Generic;
using ForestRoyale.Gameplay.Units;

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
			if (unit.CanMove)
			{
				_activeUnits.Add(unit);
			}
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
					case UnitState.CastingPreview:
					case UnitState.Idle:
					case UnitState.Attacking:
					case UnitState.Dying:
					case UnitState.Dead:
						troop.MovementComponent.Stop();
						break;
					
					case UnitState.MovingToTarget:
						troop.MovementComponent.Move();
						troop.MovementComponent.UpdateMoveDestination();
						break;
					
					default:
						UnityEngine.Debug.LogError($"Unknown unit state: {troop.State}");
						break;
				}
			}
		}

		public void Pause()
		{
			foreach (Unit troop in _activeUnits)
			{
				troop.MovementComponent.Stop();
			}
		}
	}
}