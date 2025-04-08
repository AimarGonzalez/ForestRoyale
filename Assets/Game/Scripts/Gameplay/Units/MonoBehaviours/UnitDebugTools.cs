using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Systems;
using ForestRoyale.Gameplay.Units;
using ForestRoyale.Gameplay.Units.MonoBehaviors;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Gameplay.Units.MonoBehaviours
{
	[RequireComponent(typeof(UnitRoot))]
	public class UnitDebugTools : MonoBehaviour
	{
		[BoxGroup("Unit setup")]
		[SerializeField]
		[OnValueChanged("@" + nameof(SetCardData) + "()")]
		private CardData _cardData;

		[BoxGroup("Unit setup")]
		[SerializeField]
		private bool _setOnAwake;

		[BoxGroup("Target")]
		[SerializeField]
		private UnitRoot _target;

		private UnitRoot _root;

		[Inject]
		private TargetingSystem _targetingSystem;


		private void Awake()
		{
			FetchDependencies();

			if (_setOnAwake)
			{
				SetCardData(_cardData);
			}
		}

		private void FetchDependencies()
		{
			_root ??= GetComponent<UnitRoot>();
		}

		[BoxGroup("Unit setup")]
		[Button]
		public void SetCardData()
		{
			FetchDependencies();
			if (_cardData != null)
			{
				SetCardData(_cardData);
			}
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

			Unit unit = null;

			//TODO: Use a factory to spawn the Unit from CardData
			unit = new Unit(cardData, _root, unitCard.UnitStats, unitCard.CombatStats);
			_root.SetUnit(unit);
		}

		[BoxGroup("Target")]
		[Button]
		void GoToTarget()
		{
			if (!_target)
			{
				return;
			}

			_targetingSystem.SetTarget(_root.Unit, _target.Unit);
		}
	}
}