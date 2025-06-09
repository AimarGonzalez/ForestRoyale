using ForestRoyale.Gameplay.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Gameplay.Combat
{
	[CreateAssetMenu(fileName = "DeckData", menuName = "Forest Royale/Deck Data")]
	public class DeckDataSO : ScriptableObject
	{
		[SerializeField]
		private List<CardData> _cards = new();

		public IReadOnlyList<CardData> Cards => _cards;
	}
}
