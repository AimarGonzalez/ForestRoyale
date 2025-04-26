using System.Collections.Generic;
using ForestRoyale.Gameplay.Units;
using ForestRoyale.Gameplay.Units.MonoBehaviours.Components;
using System.Linq;
using UnityEngine;

namespace ForestRoyale.Gameplay.Systems
{
	public class UnitStateMachine
	{
		private readonly ArenaEvents _arenaEvents;
		private readonly HashSet<Unit> _allUnits;

		public UnitStateMachine(ArenaEvents arenaEvents)
		{
			_arenaEvents = arenaEvents;
			_allUnits = new HashSet<Unit>();

			_arenaEvents.OnUnitCreated += HandleUnitCreated;
			_arenaEvents.OnUnitDestroyed += HandleUnitDestroyed;
		}

		private void HandleUnitCreated(Unit unit)
		{
			_allUnits.Add(unit);
		}

		private void HandleUnitDestroyed(Unit unit)
		{
			_allUnits.Remove(unit);
		}

		public void UpdateState()
		{
			// Work on cloned list to avoid invalidating iterator
			Unit[] allUnits = _allUnits.ToArray();
			
			foreach (Unit troop in allUnits)
			{
				switch (troop.State)
				{
					case UnitState.Moving:
						if (troop.HasTarget && troop.IsTargetInCombatRange)
						{
							troop.State = UnitState.Attacking;
						}
						break;

					case UnitState.Attacking:
						if (troop.CombatComponent.IsPlayingAnimation)
						{
							// Don't interrupt the attack animation
							break;
						}
						
						if (!troop.Target.IsAlive)
						{
							troop.Target = null;
							troop.State = UnitState.Moving;
							break;
						}
						
						if(!troop.IsTargetInCombatRange)
						{
							troop.State = UnitState.Moving;
							break;
						}
						
						break;

					default:
						UnityEngine.Debug.LogError($"Unknown unit state: {troop.State}");
						break;
				}
				
				if (troop.IsAlive && troop.CurrentHealth <= 0)
				{
					//TODO: Implement death effects
					//troop.State = UnitState.Dying;
					troop.State = UnitState.Dead;
				}

				if (troop.State == UnitState.Dead)
				{
					_arenaEvents.TriggerUnitDestroyed(troop);

					if (troop.DeathComponent is IDeathComponent deathComponent)
					{
						deathComponent.OnDeath();
					}
					else
					{
						Object.Destroy(troop.UnitRoot.gameObject);
					}
				}
			}
		}

	}
}