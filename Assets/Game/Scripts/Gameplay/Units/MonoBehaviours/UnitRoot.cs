using ForestRoyale.Gameplay.Cards;
using Raven.Attributes;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

		[Inject]
		private ArenaEvents _arenaEvents;

		private MovementController _movementMovementController;

		//--------------------------------
		// Properties
		//--------------------------------

		public Unit Unit => _unit;
		public MovementController MovementController => _movementMovementController;

		public Vector3 Position => transform.position;

		private void Awake()
		{
			_movementMovementController = GetComponent<MovementController>();
			OnCardDataChanged(_cardData);
			Debug.Log("Awake");
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
			OnCardDataChanged(_cardData);
		}

		private void OnCardDataChanged(CardData cardData)
		{
			Unit unit = null;
			if (cardData is not null)
			{
				IUnitCard unitCard = cardData as IUnitCard;

				//TODO: Use a factory to spawn the Unit from CardData
				unit = new Unit(cardData, this, unitCard.UnitStats, unitCard.CombatStats);
			}
			SetUnit(unit);
		}

		private void SetUnit(Unit unit)
		{
			if (_unit == unit)
			{
				return;
			}

			if (_unit != null)
			{
				_arenaEvents.TriggerUnitDestroyed(_unit);
			}

			_unit = unit;

			OnUnitChanged?.Invoke(Unit);

			if (_unit != null)
			{
				_arenaEvents.TriggerUnitCreated(_unit);
			}
		}

		void OnDrawGizmos()
		{
#if UNITY_EDITOR
			if (_unit == null)
			{
				return;
			}

			Vector3 position = transform.position;
			Vector3 labelPosition = position + Vector3.down * 0.5f;

			Handles.BeginGUI();
			try
			{
				Vector2 screenPoint = HandleUtility.WorldToGUIPoint(labelPosition);

				string info = $"[{_unit.Id}]\n";
				info += $"Target: {_unit.Target?.Id ?? "None"}\n";
				info += $"In Combat Range: {(_unit?.TargetIsInCombatRange ?? false ? "Yes" : "No")}\n";

				GUIStyle style = new GUIStyle();
				style.normal.textColor = Color.white;
				style.padding = new RectOffset(5, 5, 5, 5);

				Vector2 size = style.CalcSize(new GUIContent(info));
				Rect rect = new Rect(screenPoint.x - size.x * 0.5f, screenPoint.y, size.x, size.y);

				EditorGUI.DrawRect(rect, new Color(0, 0, 0, 0.5f));
				EditorGUI.LabelField(rect, info, style);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
			finally
			{
				Handles.EndGUI();
			}
#endif
		}
	}
}