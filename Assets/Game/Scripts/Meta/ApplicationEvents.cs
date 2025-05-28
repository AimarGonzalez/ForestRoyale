using System;
using ForestRoyale.Gameplay.Combat;

namespace ForestRoyale.Gameplay.Systems
{
	public class ApplicationEvents
	{

		public event Action<Battle> OnBattleStarted;

		public void TriggerBattleStarted(Battle battle)
		{
			OnBattleStarted?.Invoke(battle);
		}
	}
}