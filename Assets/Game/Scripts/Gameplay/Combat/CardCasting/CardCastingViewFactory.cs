using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Units;
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
		private TroopCastingView _troopCastingViewPrefab;

		[Inject]
		private IObjectResolver _container;

		public ICastingView BuildCastingPreview(CardData cardData, ArenaTeam team, ICastingView castingView)
		{
			switch (cardData)
			{
				case TroopCardData troopCard:
					//TODO: pool casting views, instead of caching them
					return BuildTroopCastingPreview(troopCard, team, castingView as TroopCastingView);

				case SpellCardData spellCard:
					//TODO: Implement
					return null;

				default:
					Debug.LogError($"{nameof(CardCastingViewFactory)} - BuildCastingPreview:"
										+ $" Card {cardData.CardName} is not a valid card type ({cardData.GetType().Name})");
					return null;
			}
		}

		private ICastingView BuildTroopCastingPreview(TroopCardData troopCard, ArenaTeam team, TroopCastingView troopCastingView = null)
		{
			troopCastingView ??= _container.Instantiate(_troopCastingViewPrefab, _castingArea);
			GameObject troopInstance = _container.Instantiate(troopCard.UnitPrefab);
			troopCastingView.SetTroop(troopCard, troopInstance.transform, team, UnitState.CastingPreview);

			return troopCastingView;
		}
	}
}
