using ForestRoyale.Core.UI;
using ForestRoyale.Gameplay.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Gameplay.Combat
{
	[CreateAssetMenu(fileName = "DeckData", menuName = "ForestRoyale/Deck Data", order = ToolConstants.RootMenuOrder)]
	public class DeckDataSO : ScriptableObject
	{
		[SerializeField]
		private List<CardData> _cards = new();

		public IReadOnlyList<CardData> Cards => _cards;
	}
}
