using UnityEngine;
using ForestRoyale.Gameplay.Cards.CardStats;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewTroop", menuName = "ForestRoyale/Troop Data", order = 1)]
	public class TroopCardData : CardData, IUnitCard
	{
		[Tooltip("Number of units in this card")]
		[SerializeField]
		private int _unitCount = 1;

		[SerializeField]
		private TroopStats _troopStats;

		[SerializeField]
		private CombatStats _combatStats;

		public CombatStats CombatStats => _combatStats;
		public TroopStats TroopStats => _troopStats;
		public int UnitCount => _unitCount;

		// Implement IUnitCard interface
		public UnitStats UnitStats => _troopStats;

#if UNITY_EDITOR
		public static TroopCardData Build(
			string cardName,
			string description,
			Sprite portrait,
			int elixirCost,
			CardRarity rarity,
			int unitCount,
			TroopStats troopStats,
			CombatStats combatStats)
		{
			TroopCardData card = CreateInstance<TroopCardData>();
			card.SetCardData(cardName, description, portrait, elixirCost, rarity);

			card._unitCount = unitCount;
			card._troopStats = troopStats;
			card._combatStats = combatStats;

			return card;
		}
#endif
	}
}