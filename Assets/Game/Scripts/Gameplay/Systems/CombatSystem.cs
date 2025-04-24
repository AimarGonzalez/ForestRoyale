using ForestRoyale.Gameplay.Units;
using System.Collections.Generic;

namespace ForestRoyale.Gameplay.Systems
{
	public class CombatSystem
	{

		public struct HitData
		{
			public Unit Attacker;
			public List<Unit> Targets;
			public float Damage;
		}

		private readonly ArenaEvents _arenaEvents;
		private readonly HashSet<Unit> _allUnits;
		private readonly HashSet<Unit> _combatUnits;

		private readonly List<HitData> _hits = new List<HitData>();

		public CombatSystem(ArenaEvents arenaEvents)
		{
			_arenaEvents = arenaEvents;
			_allUnits = new HashSet<Unit>();

			_arenaEvents.OnUnitCreated += HandleUnitCreated;
			_arenaEvents.OnUnitDestroyed += HandleUnitDestroyed;
			_arenaEvents.OnUnitAttacked += HandleUnitAttacked;
		}

		private void HandleUnitCreated(Unit unit)
		{
			_allUnits.Add(unit);
			
			if (unit.CanFight)
			{
				_combatUnits.Add(unit);
			}
		}

		private void HandleUnitDestroyed(Unit unit)
		{
			_allUnits.Remove(unit);
			_combatUnits.Remove(unit);
		}

		private void HandleUnitAttacked(Unit attacker, Unit target)
		{
			List<Unit> targets = new List<Unit> { target }; // calculate targets (area of effect)
			_hits.Add(new HitData { Attacker = attacker, Targets = targets, Damage = attacker.CombatStats.Damage });
		}



		public void UpdateCombat()
		{
			foreach (Unit troop in _combatUnits)
			{
				if (troop.State == UnitState.Attacking && troop.TargetIsInCombatRange)
				{
					troop.CombatComponent.Attack();
				}
			}

			foreach (HitData hit in _hits)
			{
				foreach (Unit target in hit.Targets)
				{
					target.TakeDamage(hit);
				}
			}
		}

	}
}