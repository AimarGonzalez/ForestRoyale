using ForestRoyale.Gameplay.Cards.ScriptableObjects;
using ForestRoyale.Gameplay.Systems;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using VContainer;
using ForestRoyale.Gui;
using Sirenix.Utilities.Editor;
using System.Linq;
using UnityEngine.Assertions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ForestRoyale.Gameplay.Units.MonoBehaviors
{
	public class UnitRoot : MonoBehaviour
	{
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
		private GUIUtils.PanelPosition _panelPosition;


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
			GUIUtils.Property[] properties;
			if (_unit == null)
			{
				properties = new[] { new GUIUtils.Property("null unit") };
			}
			else
			{
				properties = new[] {
					new GUIUtils.Property ("Id", _unit.Id.ToString()),
					new GUIUtils.Property ("Target", _unit.Target?.Id.ToString() ?? "None"),
					_unit.TargetIsInCombatRange ?
						new GUIUtils.Property ("In Range", "Yes", GuiStylesCatalog.LabelGreenStyle) :
						new GUIUtils.Property ("In Range", "No", GuiStylesCatalog.LabelRedStyle)
				};
			}
			DrawDebugPanel(properties);
		}

		private void DrawDebugPanel(GUIUtils.Property[] properties)
		{
			GUIStyle panelStyle = GuiStylesCatalog.DebugPanelStyle;

			(Vector2 panelSize, float labelWidth) = GUIUtils.CalcPanelSize(panelStyle, properties);
			Vector3 panelPosition = GUIUtils.CalcPanelPosition(transform, panelSize, _panelPosition);

			Handles.BeginGUI();
			{
				// Create rect centered on the panel's position
				Rect rect = new Rect(panelPosition.x - panelSize.x * 0.5f, panelPosition.y - panelSize.y * 0.5f, panelSize.x, panelSize.y);

				GUI.Box(rect, GUIContent.none, panelStyle);

				for (int i = 0; i < properties.Length; i++)
				{
					GUIUtils.DrawTextField(i, properties[i], rect, labelWidth, panelStyle);
				}
			}
			Handles.EndGUI();
		}
#endif //UNITY_EDITOR
	}
}