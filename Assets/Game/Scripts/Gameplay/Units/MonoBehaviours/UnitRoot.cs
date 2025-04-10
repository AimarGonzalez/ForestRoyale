using ForestRoyale.Gameplay.Cards.ScriptableObjects;
using ForestRoyale.Gameplay.Systems;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using VContainer;
using ForestRoyale.Gui;
using UnityEngine.Assertions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ForestRoyale.Gameplay.Units.MonoBehaviors
{
	public class UnitRoot : MonoBehaviour
	{
		private enum PanelPosition
		{
			Bottom,
			Top,
			Left,
			Right
		}

		public Action<Unit> OnUnitChanged;

		[SerializeField]
		private ArenaTeam _team;
		[SerializeField]
		private UnitSO _startingUnitSO;

		[ShowInInspector, ReadOnly]
		[BoxGroup(InspectorConstants.DebugBoxGroup), PropertyOrder(InspectorConstants.DebugBoxGroupOrder)]
		[NonSerialized]
		private Unit _unit;

		[SerializeField]
		[BoxGroup(InspectorConstants.DebugBoxGroup), PropertyOrder(InspectorConstants.DebugBoxGroupOrder)]
		private PanelPosition _panelPosition;


		[Inject]
		private ArenaEvents _arenaEvents;

		private MovementController _movementMovementController;

		//--------------------------------
		// Properties
		//--------------------------------

		public ArenaTeam Team => _team;
		public Unit Unit => _unit;
		public MovementController MovementController => _movementMovementController;

		public Vector3 Position => transform.position;

		private void Awake()
		{
			_movementMovementController = GetComponent<MovementController>();

			Assert.IsNotNull(_startingUnitSO, "startingUnitSO is not set");
			if (_startingUnitSO != null)
			{
				//TODO: Use a factory to spawn the Unit from CardData
				Unit unit = new Unit(null, this, _team, _startingUnitSO);
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


#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			string info = "";
			if (_unit == null)
			{
				info = "null unit";
			}
			else
			{
				info = $"[{_unit.Id}]\n";
				info += $"Target: {_unit.Target?.Id ?? "None"}\n";
				info += $"In Combat Range: {(_unit?.TargetIsInCombatRange ?? false ? "Yes" : "No")}";
			}

			DrawDebugPanel(info);
		}

		private void DrawDebugPanel(string info)
		{
			Vector2 screenOffset = Vector2.zero;
			Vector3 worldOffset = Vector3.zero;

			float baseWorldOffset = 1f; //1 meter - TODO: Calculate based on the unit bounding box.

			GUIStyle panelStyle = GuiStylesCatalog.DebugPanelStyle;

			Vector2 size = panelStyle.CalcSize(new GUIContent(info));

			switch (_panelPosition)
			{
				case PanelPosition.Top:
					screenOffset = Vector2.down * (size.y * 0.5f);
					worldOffset = Vector3.forward * baseWorldOffset;
					break;
				case PanelPosition.Bottom:
					screenOffset = Vector2.up * (size.y * 0.5f);
					worldOffset = Vector3.back * baseWorldOffset;
					break;
				case PanelPosition.Left:
					screenOffset = Vector2.left * (size.x * 0.5f);
					worldOffset = Vector3.left * baseWorldOffset;
					break;
				case PanelPosition.Right:
					screenOffset = Vector2.right * (size.x * 0.5f);
					worldOffset = Vector3.right * baseWorldOffset;
					break;
			}

			Vector3 worldPosition = transform.position + worldOffset;
			Vector2 screenPoint = HandleUtility.WorldToGUIPoint(worldPosition);
			Vector3 labelPosition = screenPoint + screenOffset;

			Handles.BeginGUI();
			{
				// Create rect centered on the labelPosition
				Rect rect = new Rect(labelPosition.x - size.x * 0.5f, labelPosition.y - size.y * 0.5f, size.x, size.y);
				GUI.Box(rect, info, panelStyle);
			}
			Handles.EndGUI();
		}
#endif //UNITY_EDITOR
	}
}