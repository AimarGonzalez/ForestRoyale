using UnityEngine;
using ForestRoyale.Gameplay.Cards.ScriptableObjects;
using Sirenix.OdinInspector;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewTroop", menuName = "ForestRoyale/Troop Data", order = 1)]
	public class TroopCardData : CardData, IUnitCard
	{
		[Tooltip("Number of units in this card")]
		[BoxGroup("Troop")]
		[SerializeField]
		private int _unitCount = 1;

		[SerializeField, InlineEditor]
		[BoxGroup("Troop")]
		private UnitSO _unitSO;

		public int UnitCount => _unitCount;

		// Implement IUnitCard interface
		public UnitSO UnitSO => _unitSO;

#if UNITY_EDITOR
		public static TroopCardData Build(
			string cardName,
			string description,
			Sprite portrait,
			int elixirCost,
			CardRarity rarity,
			int unitCount,
			UnitSO unitSO)
		{
			TroopCardData card = CreateInstance<TroopCardData>();
			card.SetCardData(cardName, description, portrait, elixirCost, rarity);

			card._unitCount = unitCount;
			card._unitSO = unitSO;

			return card;
		}
#endif
	}
}