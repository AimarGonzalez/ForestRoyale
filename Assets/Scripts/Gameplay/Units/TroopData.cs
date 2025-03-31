using ForestRoyale.Gameplay.Cards;


namespace ForestRoyale.Gameplay.Units
{
	public class TroopData : IDamageable
	{
		private float _remainingHealth;

		public bool IsPlayerTeam;
		public bool IsForestTeam => !IsPlayerTeam;

		public readonly IUnitCard _cardReference;
		public readonly UnitStats _unitStats;
		public float MaxHealth => _unitStats.HitPoints;
		public float RemainingHealth => _remainingHealth;


		public float HealthRatio => RemainingHealth / MaxHealth;

		public TroopData Target;

		public TroopData(IUnitCard card) : this(card.UnitStats)
		{
			_cardReference = card;
		}

		public TroopData(UnitStats unitStats)
		{
			_unitStats = unitStats;
			_remainingHealth = unitStats.HitPoints;
		}
	}
}

