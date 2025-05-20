using System;
using ForestRoyale.Gameplay.Units;

namespace ForestRoyale.Gameplay.Systems
{
	public class ApplicationEvents
	{

		public event Action<Unit> OnBattleStarted;

		public void TriggerBattleStarted(Unit unit)
		{
			OnBattleStarted?.Invoke(unit);
		}
	}
}