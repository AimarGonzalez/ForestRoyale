using ForestRoyale.Core.Pool;
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
		
		[Inject]
		private GameObjectPoolService _poolService;

		public ICastingView BuildCastingPreview(CardData cardData, ArenaTeam team)
		{
			switch (cardData)
			{
				case TroopCardData troopCard:
					//TODO: pool casting views, instead of caching them
					return BuildTroopCastingPreview(troopCard, team);

				case SpellCardData spellCard:
					//TODO: Implement
					return null;

				default:
					Debug.LogError($"{nameof(CardCastingViewFactory)} - BuildCastingPreview:"
										+ $" Card {cardData.CardName} is not a valid card type ({cardData.GetType().Name})");
					return null;
			}
		}

		private ICastingView BuildTroopCastingPreview(TroopCardData troopCard, ArenaTeam team)
		{
			TroopCastingView troopCastingView = _poolService.Get(_troopCastingViewPrefab, _castingArea);
 
			if (troopCastingView.State == TroopCastingView.CastingState.Empty)
			{
				PooledGameObject troopInstance = _poolService.Get(troopCard.UnitSO.Prefabs.UnitPrefab);
				troopCastingView.SetTroop(troopCard, troopInstance, team, UnitState.CastingPreview);
			}

			return troopCastingView;
		}
	}
}
