using ForestRoyale.Gameplay.Cards;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ForestRoyale.Gameplay.Combat
{
	public class CardCastingViewFactory
	{
		[SerializeField]
		private GameObject _troopCastingUIPrefab;

		[Inject]
		private IObjectResolver _container;


		public GameObject BuildCastingPreview(CardData cardData)
		{
			switch (cardData)
			{
				case TroopCardData troopCard:
					return BuildTroopCastingPreview(troopCard);

				case SpellCardData spellCard:
					//TODO: Implement
					return null;

				default:
					Debug.LogError($"{nameof(CardCastingViewFactory)} - BuildCastingPreview:"
										+ $" Card {cardData.CardName} is not a valid card type ({cardData.GetType().Name})");
					return null;
			}
		}

		private GameObject BuildTroopCastingPreview(TroopCardData troopCard)
		{
			GameObject troopInstance = _container.Instantiate(troopCard.UnitPrefab);
			GameObject castingView = _container.Instantiate(_troopCastingUIPrefab);
			TroopCastingView troopCastingView = castingView.GetComponent<TroopCastingView>();
			troopCastingView.SetTroop(troopCard, troopInstance);

			return castingView;
		}
	}
}
