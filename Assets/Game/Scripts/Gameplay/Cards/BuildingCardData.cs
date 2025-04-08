using ForestRoyale.Gameplay.Cards.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ForestRoyale.Gameplay.Cards
{
	[CreateAssetMenu(fileName = "NewBuilding", menuName = "ForestRoyale/Building Data", order = 1)]
	public class BuildingCardData : CardData, IUnitCard
	{
		[SerializeField, InlineEditor] private UnitSO _unitSO;

		// Implement IUnitCard interface	
		public UnitSO UnitSO => _unitSO;

#if UNITY_EDITOR
		public static BuildingCardData Build(
			string cardName,
			string description,
			Sprite portrait,
			int elixirCost,
			CardRarity rarity,
			UnitSO unitSO)
		{
			BuildingCardData card = CreateInstance<BuildingCardData>();
			card.SetCardData(cardName, description, portrait, elixirCost, rarity);

			card._unitSO = unitSO;

			return card;
		}
#endif
	}
}