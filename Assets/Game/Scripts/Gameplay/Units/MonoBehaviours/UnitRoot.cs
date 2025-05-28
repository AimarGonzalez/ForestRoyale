using ForestRoyale.Core.UI;
using ForestRoyale.Gameplay.Cards.ScriptableObjects;
using ForestRoyale.Gameplay.Systems;
using ForestRoyale.Gameplay.Units.MonoBehaviours.Components;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours
{
	// Add ExecuteInEditMode so OnGUI draws the debug panel in the editor
	[ExecuteInEditMode]
	public class UnitRoot : MonoBehaviour
	{
		public Action<Unit> OnUnitChanged;

		[SerializeField]
		private ArenaTeam _team;
		[SerializeField]
		private UnitSO _startingUnitSO;

		[ShowInInspector, DisableInEditorMode]
		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[NonSerialized]
		private Unit _unit;

		[SerializeField]
		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private bool _showDebugPanel = false;

		[SerializeField]
		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private GUIUtils.PanelPlacement _panelPosition;

		[SerializeField]
		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private float _panelMargin = 0f;


		[Inject]
		private ArenaEvents _arenaEvents;

		private MovementComponent _movementComponent;
		private CombatComponent _combatComponent;
		private IDeathComponent _deathComponent;
		private Collider2DListener _colliderListener;


		//--------------------------------
		// Properties
		//--------------------------------

		public ArenaTeam Team => _team;
		public Unit Unit => _unit;
		public MovementComponent MovementComponent => _movementComponent;
		public CombatComponent CombatComponent => _combatComponent;
		public IDeathComponent DeathComponent => _deathComponent;
		public Vector3 Position => transform.position;

		private void Awake()
		{
			_movementComponent = GetComponent<MovementComponent>();
			_combatComponent = GetComponent<CombatComponent>();
			_deathComponent = GetComponent<IDeathComponent>();

			if (_movementComponent)
			{
				_colliderListener = _movementComponent.Body.GetComponent<Collider2DListener>();
				Assert.IsNotNull(_colliderListener);
			}

#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				ForceAwakeSubComponents();
			}
#endif
		}

		private void Start()
		{
			InitializeUnit();

			Subscribe();
		}

		private void InitializeUnit()
		{
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


		private void Subscribe()
		{
			if (_colliderListener != null)
			{
				_colliderListener.OnMouseDownEvent += OnMouseDown;
			}
		}

		private void Unsubscribe()
		{
			if (_colliderListener != null)
			{
				_colliderListener.OnMouseDownEvent -= OnMouseDown;
			}
		}

		private void OnDestroy()
		{
			Unsubscribe();

#if UNITY_EDITOR
			ForceOnDestroySubComponents();
#endif
		}

		private void OnMouseDown()
		{
			_showDebugPanel = !_showDebugPanel;
		}

#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			// Handles.BeginGUI();
			// DrawDebugGUI();
			// Handles.EndGUI();
		}

		void OnGUI()
		{
			DrawDebugGUI();
		}

		private void DrawDebugGUI()
		{
			if (!_showDebugPanel)
			{
				return;
			}

			GUIUtils.Property[] properties;
			if (_unit == null)
			{
				properties = new[] { new GUIUtils.Property("null unit") };
			}
			else
			{
				properties = new[] {
					new GUIUtils.Property ("Id", _unit.Id),
					new GUIUtils.Property ("Target", _unit.Target?.Id ?? "None"),
					Application.isPlaying && _unit.IsTargetInCombatRange?
						new GUIUtils.Property ("In Range", "Yes", GuiStylesCatalog.LabelGreenStyle) :
						new GUIUtils.Property ("In Range", "No", GuiStylesCatalog.LabelRedStyle),
					new GUIUtils.Property ("State", _unit.State)
				};

				if (_unit.CanFight)
				{
					properties = properties
						.Append(new GUIUtils.Property("CombatState", _unit.CombatComponent.State.ToString()))
						.Append(new GUIUtils.Property("  Cooldown", _unit.CombatComponent.Cooldown.ToString("F2")))
						.ToArray();
				}
			}

			GUIUtils.DrawDebugPanel(properties, transform, _panelPosition, _panelMargin, () => _showDebugPanel = false);
		}
		
		[Button]
		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order-1)]
		private void ForceInitializeUnit()
		{
			ForceAwakeSubComponents();
			InitializeUnit();
		}

		private void ForceAwakeSubComponents()
		{
			var unitComponents = GetComponentsInChildren<UnitComponent>();
			foreach (var component in unitComponents)
			{
				component.ForceAwake(this);
			}
		}

		private void ForceOnDestroySubComponents()
		{
			var unitComponents = GetComponentsInChildren<UnitComponent>();
			foreach (var component in unitComponents)
			{
				component.ForceOnDestroy();
			}
		}
#endif //UNITY_EDITOR
	}
}