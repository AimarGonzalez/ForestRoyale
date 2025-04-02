using ForestRoyale.Gameplay.Cards;
using Raven.Attributes;
using System;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units.MonoBehaviors
{
	public class UnitRoot : MonoBehaviour
	{
		public Action<Unit> OnUnitChanged;
		
		[SerializeField] 
		[OnValueChanged("SetCardData")]
		private CardData _cardData;

		[SerializeField]
		private Unit _unit;
		
		private MovementController _movementMovementController;

		public Unit Unit => _unit;
		public MovementController MovementController => _movementMovementController;
		
		public Vector3 Position => transform.position;

		private void Awake()
		{
			_movementMovementController = GetComponent<MovementController>();
		}

		[Button]
		public void SetCardData()
		{
			SetCardData(_cardData);
		}
		public void SetCardData(CardData cardData)
		{
			if (cardData == null)
			{
				Debug.LogError($"{nameof(cardData)} is null");
				return;
			}

			if (cardData is not IUnitCard unitCard)
			{
				Debug.LogError($"{nameof(cardData)} is not an IUnitCard (its a {cardData.GetType().Name})");
				return;
			}

			_cardData = cardData;
			OnCardDataChanged(cardData);
		}

		private void OnCardDataChanged(CardData cardData)
		{
			_cardData = cardData;
			
			IUnitCard unitCard = cardData as IUnitCard;
			SetUnit(new Unit(cardData, this, unitCard.UnitStats, unitCard.CombatStats));
		}

		private void SetUnit(Unit unit)
		{
			if (_unit == unit)
			{
				return;
			}
			
			_unit = unit;
			OnUnitChanged?.Invoke(Unit);
		}
	}
}