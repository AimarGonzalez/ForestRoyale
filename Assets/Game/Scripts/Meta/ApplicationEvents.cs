using System;
using ForestRoyale.Gameplay.Combat;

namespace ForestRoyale.Gameplay.Systems
{
	public class ApplicationEvents
	{

		public event Action<Battle> OnBattleCreated;
		public event Action<Battle> OnBattleStarted;
		public event Action<Battle> OnBattleEnded;

		public void TriggerBattleCreated(Battle battle)
		{
			OnBattleCreated?.Invoke(battle);
		}

		public void TriggerBattleStarted(Battle battle)
		{
			OnBattleStarted?.Invoke(battle);
		}

		public void TriggerBattlePaused(Battle battle)
		{
			OnBattleEnded?.Invoke(battle);
		}
	}
}