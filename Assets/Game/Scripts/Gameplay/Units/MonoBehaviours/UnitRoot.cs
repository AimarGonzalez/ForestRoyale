using ForestLib.ExtensionMethods;
using ForestRoyale.Core.Pool;
using ForestRoyale.Core.UI;
using ForestRoyale.Gameplay.Cards.ScriptableObjects;
using ForestRoyale.Gameplay.Systems;
using ForestRoyale.Gameplay.Units.MonoBehaviours.Components;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using VContainer;
using VContainer.Unity;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours
{
	// Add ExecuteInEditMode so OnGUI draws the debug panel in the editor
	[ExecuteInEditMode]
	public class UnitRoot : PooledGameObject
	{
		[SerializeField]
		private ArenaTeam _startingTeam;
		[SerializeField]
		private UnitSO _startingUnitSO;
		[SerializeField]
		private UnitState _startingState = UnitState.Idle;

		// --- DEGUG BOX ---------------


		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order - 2)]
		[ShowInInspector]
		private UnitState State => _unit?.State ?? UnitState.None;

		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order - 2)]
		[ShowInInspector]
		private string TargetId => _unit == null ? "<no unit>" : (_unit.Target?.Id ?? "<no target>");

		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[ShowInInspector]
		[NonSerialized]
		private Unit _unit;

		[SerializeField]
		[BoxGroup("Debug Panel"), PropertyOrder(9999)]
		private bool _showDebugPanel = false;

		[SerializeField]
		[BoxGroup("Debug Panel"), PropertyOrder(9999)]
		private GUIUtils.PanelPlacement _panelPosition;

		[SerializeField]
		[BoxGroup("Debug Panel"), PropertyOrder(9999)]
		private float _panelMargin = 0f;


		[Inject]
		private ArenaEvents _arenaEvents;

		[Inject]
		private IObjectResolver _objectResolver;

		private MovementComponent _movementComponent;
		private CombatComponent _combatComponent;
		private DeathComponent _deathComponent;
		private Collider2DListener _colliderListener;

		[NonSerialized]
		private List<UnitComponent> _unitComponents = null;

		[NonSerialized]
		private List<IUnitChangeListener> _unitChangeListeners = null;

		[NonSerialized]
		private List<IUnitStateChangeListener> _unitStateListeners = null;

		private List<UnitComponent> UnitComponents
		{
			get
			{
				if (_unitComponents == null)
				{
					_unitComponents = GetComponentsInChildren<UnitComponent>().ToList();
				}
				return _unitComponents;
			}
		}

		private List<IUnitChangeListener> UnitListeners
		{
			get
			{
				if (_unitChangeListeners == null)
				{
					_unitChangeListeners = GetComponentsInChildren<IUnitChangeListener>().ToList();
				}
				return _unitChangeListeners;
			}
		}


		private List<IUnitStateChangeListener> UnitStateListeners
		{
			get
			{
				if (_unitStateListeners == null)
				{
					_unitStateListeners = GetComponentsInChildren<IUnitStateChangeListener>().ToList();
				}
				return _unitStateListeners;
			}
		}


		//--------------------------------
		// Properties
		//--------------------------------

		public ArenaTeam StartingTeam
		{
			get => _startingTeam;
			set
			{
				// can be set from the TroopCastingView
				_startingTeam = value;
			}
		}

		public UnitState StartingState
		{
			get => _startingState;
			set
			{
				// can be set from the TroopCastingView
				_startingState = value;
			}
		}

		public Unit Unit => _unit;
		public MovementComponent MovementComponent => _movementComponent;
		public CombatComponent CombatComponent => _combatComponent;
		public DeathComponent DeathComponent => _deathComponent;
		public Vector3 Position => transform.position;

		private void Awake()
		{
#if UNITY_EDITOR
			// VContainer injection for prefabs added from the editor.
			AutoInject();
#endif

			_movementComponent = GetComponent<MovementComponent>();
			_combatComponent = GetComponent<CombatComponent>();
			_deathComponent = GetComponent<DeathComponent>();

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
			if (!CreatedOnPool)
			{
				OnBeforeGetFromPool();
				OnAfterGetFromPool();
			}
		}
		
		
		private void OnDestroy()
		{
			if (!CreatedOnPool)
			{
				OnDestroyFromPool();
			}
		}
		
		// -------- Poolable ---------------------------------------
		public void OnBeforeGetFromPool()
		{
			CreateUnit();
		}
		public void OnAfterGetFromPool()
		{
			Subscribe();
		}
		public void OnReturnToPool()
		{
			Unsubscribe();
			NotifyUnitRemoved();
		}
		public void OnDestroyFromPool()
		{
			OnReturnToPool();
		}
		// ----------------------------------------------------------
		

		private void CreateUnit()
		{
			Assert.IsNotNull(_startingUnitSO, "startingUnitSO is not set");
			if (_startingUnitSO != null)
			{
				//TODO: Use a factory to spawn the Unit from CardData
				Unit unit = new(null, this, _startingTeam, _startingUnitSO, _startingState);
				SetUnit(unit);
			}
		}

		public void NotifyUnitRemoved()
		{
			if (_unit != null)
			{
				_arenaEvents?.TriggerUnitRemoved(_unit);
				_unit = null;
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

		public void SetUnit(Unit unit)
		{
			if (_unit == unit)
			{
				return;
			}

			Unit oldUnit = _unit;
			UnitState oldUnitState = oldUnit?.State ?? UnitState.None;

			NotifyUnitRemoved();

			_unit = unit;

			UpdateUnitComponents(unit);
			PropagateUnitChanged(oldUnit, unit);
			PropagateStateChanged(oldUnitState, unit.State);

			if (unit.State == UnitState.Idle)
			{
				_arenaEvents?.TriggerUnitCreated(_unit);
			}
		}

		public void SetState(UnitState newState)
		{
			if (_unit == null)
			{
				return;
			}

			UnitState oldState = _unit.State;

			_unit.State = newState;
			// PropagateStateChanged is called inside Unit.State setter.

			if (oldState == UnitState.CastingPreview && newState == UnitState.Idle)
			{
				_arenaEvents?.TriggerUnitCreated(_unit);
			}
		}
		
		private void UpdateUnitComponents(Unit newUnit)
		{
			foreach (var component in UnitComponents)
			{
				component.SetUnit(newUnit);
			}
		}

		private void PropagateUnitChanged(Unit oldUnit, Unit newUnit)
		{
			foreach (var listener in UnitListeners)
			{
				listener.OnUnitChanged(oldUnit, newUnit);
			}
		}

		public void PropagateStateChanged(UnitState oldState, UnitState newState)
		{
			foreach (var listener in UnitStateListeners)
			{
				listener.OnUnitStateChanged(oldState, newState);
			}
		}

		private void OnMouseDown()
		{
			_showDebugPanel = !_showDebugPanel;
		}

#if UNITY_EDITOR
		// VContainer injection for prefabs added from the editor.
		private void AutoInject()
		{
			if (!Application.isPlaying)
			{
				// Inject only in playMode
				return;
			}

			if (_objectResolver == null)
			{
				_objectResolver = FindFirstObjectByType<GameBootstrap>().Container;
			}

			_objectResolver.InjectGameObject(gameObject);
		}
#endif

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

				if (_unit.CanFight && _unit.State == UnitState.Attacking)
				{
					properties = properties
						.Append(new GUIUtils.Property("CombatState", _unit.CombatComponent.State.ToString()))
						.Append(new GUIUtils.Property("  Cooldown", _unit.CombatComponent.Cooldown.ToString("F2")))
						.ToArray();
				}
			}

			GUIUtils.DrawDebugPanel(properties, transform, _panelPosition, _panelMargin, () => _showDebugPanel = false);
		}

#if UNITY_EDITOR
		[Button]
		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order - 1)]
		private void ForceInitializeUnit()
		{
			ForceAwakeSubComponents();
			CreateUnit();
		}

		private void ForceAwakeSubComponents()
		{
			foreach (var component in UnitComponents)
			{
				component.ForceAwake(this);
			}
		}
#endif //UNITY_EDITOR
	}
}