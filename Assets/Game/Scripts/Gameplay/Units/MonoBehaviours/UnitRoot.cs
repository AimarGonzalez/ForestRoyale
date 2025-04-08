using ForestRoyale.Gameplay.Systems;
using ForestRoyale.Gameplay.Cards.ScriptableObjects;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using VContainer;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ForestRoyale.Gameplay.Units.MonoBehaviors
{
	public class UnitRoot : MonoBehaviour
	{
		public Action<Unit> OnUnitChanged;

		[ShowInInspector, ReadOnly]
		private Unit _unit;

		[SerializeField]
		private UnitSO _startingUnitSO;

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

			if (_startingUnitSO != null)
			{
				//TODO: Use a factory to spawn the Unit from CardData
				Unit unit = new Unit(null, this, _startingUnitSO.UnitStats, _startingUnitSO.CombatStats);
				SetUnit(unit);
			}
		}

		public void SetUnit(Unit unit)
		{
			if (_unit == unit)
			{
				return;
			}

			if (_unit != null && Application.isPlaying)
			{
				_arenaEvents.TriggerUnitDestroyed(_unit);
			}

			_unit = unit;

			OnUnitChanged?.Invoke(Unit);

			if (_unit != null && Application.isPlaying)
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