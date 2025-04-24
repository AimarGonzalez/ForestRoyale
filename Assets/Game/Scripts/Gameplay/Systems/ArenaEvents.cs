using System;
using ForestRoyale.Gameplay.Units;

namespace ForestRoyale.Gameplay.Systems
{
	public class ArenaEvents
	{

		public event Action<Unit> OnUnitCreated;
		public event Action<Unit> OnUnitDestroyed;
		public event Action<Unit> OnUnitDamaged;
		public event Action<Unit, Unit> OnUnitAttacked;

		public void TriggerUnitCreated(Unit unit)
		{
			OnUnitCreated?.Invoke(unit);
		}

		public void TriggerUnitDestroyed(Unit unit)
		{
			OnUnitDestroyed?.Invoke(unit);
		}

		public void TriggerUnitDamaged(Unit unit)
		{
			OnUnitDamaged?.Invoke(unit);
		}
		
		public void TriggerUnitAttacked(Unit attacker, Unit target)
		{
			OnUnitAttacked?.Invoke(attacker, target);
		}
	}
}