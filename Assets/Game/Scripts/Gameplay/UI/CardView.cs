using ForestRoyale.Gameplay.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class CardView : MonoBehaviour
	{
		[SerializeField] private Image cardPicture;
		[SerializeField] private TMP_Text cardName;
		[SerializeField] private TMP_Text elixirCost;

		[SerializeField] private CardData _cardData;

		public CardData CardData
		{
			get => _cardData;
			set
			{
				_cardData = value;
				UpdateView();
			}
		}

		private void Start()
		{
			UpdateView();
		}

		private void OnValidate()
		{
			UpdateView();
		}

		private void UpdateView()
		{
			if (_cardData == null)
			{
				return;
			}

			if (cardPicture != null)
			{
				cardPicture.sprite = _cardData.Portrait;
			}

			if (cardName != null)
			{
				cardName.text = _cardData.CardName;
			}

			if (elixirCost != null)
			{
				elixirCost.text = _cardData.ElixirCost.ToString();
			}
		}
	}
}