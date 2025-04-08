using System;
using ForestRoyale.Gameplay.Units;

namespace ForestRoyale.Gameplay.Systems
{
	public class ArenaEvents
	{

		public event Action<Unit> OnUnitCreated;
		public event Action<Unit> OnUnitDestroyed;
		public event Action<Unit> OnUnitDamaged;

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
	}
}