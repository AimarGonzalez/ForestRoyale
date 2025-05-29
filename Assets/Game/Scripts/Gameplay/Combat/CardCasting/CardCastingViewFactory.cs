using ForestRoyale.Gameplay.Cards;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ForestRoyale.Gameplay.Combat
{
	public class CardCastingViewFactory : MonoBehaviour
	{
		[SerializeField, Required]
		private Transform _castingArea;
		
		[SerializeField]
		private GameObject _troopCastingViewPrefab;

		[Inject]
		private IObjectResolver _container;

		public ICastingView BuildCastingPreview(CardData cardData)
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

		private ICastingView BuildTroopCastingPreview(TroopCardData troopCard)
		{
			GameObject troopInstance = _container.Instantiate(troopCard.UnitPrefab);
			GameObject castingView = _container.Instantiate(_troopCastingViewPrefab, _castingArea);
			TroopCastingView troopCastingView = castingView.GetComponent<TroopCastingView>();
			troopCastingView.SetTroop(troopCard, troopInstance);

			return troopCastingView;
		}
	}
}
