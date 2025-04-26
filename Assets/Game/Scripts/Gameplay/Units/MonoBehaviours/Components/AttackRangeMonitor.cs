using ForestRoyale.Gui;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class AttackRangeMonitor : UnitComponent
	{
		[SerializeField]
		[Required]
		private CapsuleCollider _attackCollider;

		[ShowInInspector]
		[BoxGroup(InspectorConstants.DebugGroup), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		private Unit _target;

		[ShowInInspector]
		[BoxGroup(InspectorConstants.DebugGroup), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		private bool _isTargetInCombatRange = false;

		private TriggerListener _targetListener;

		public Unit Target
		{
			get => _target;
			set
			{
				_target = value;
				_isTargetInCombatRange = false;
			}
		}

		public bool IsTargetInCombatRange => _isTargetInCombatRange;

		public Action OnTargetInRangeChanged;

		protected override void Awake()
		{
			base.Awake();

			if (_attackCollider != null)
			{
				_targetListener = _attackCollider.gameObject.AddComponent<TriggerListener>();
				_targetListener.OnTriggerEnterEvent += HandleTriggerEnter;
				_targetListener.OnTriggerExitEvent += HandleTriggerExit;
				_targetListener.OnTriggerStayEvent += HandleTriggerStay;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (_attackCollider != null)
			{
				if (_targetListener != null)
				{
					_targetListener.OnTriggerEnterEvent -= HandleTriggerEnter;
					_targetListener.OnTriggerExitEvent -= HandleTriggerExit;
					_targetListener.OnTriggerStayEvent -= HandleTriggerStay;
				}
			}
		}

		protected override void OnUnitChanged()
		{
			if (Unit != null)
			{
				_attackCollider.radius = Unit.CombatStats.AttackRange;
			}
			else
			{
				// Disable target detection
				_attackCollider.radius = 0f;
			}
		}

		private void HandleTriggerEnter(Collider other)
		{
			if (_target == null)
			{
				return;
			}

			if (other.IsBodyCollider() && other.GetUnit() == _target)
			{
				_isTargetInCombatRange = true;
				OnTargetInRangeChanged?.Invoke();
			}
		}

		private void HandleTriggerStay(Collider other)
		{
			if (_target == null)
			{
				return;
			}

			if (other.IsBodyCollider() && other.GetUnit() == _target)
			{
				_isTargetInCombatRange = true;
				OnTargetInRangeChanged?.Invoke();
			}
		}

		private void HandleTriggerExit(Collider other)
		{
			if (_target == null)
			{
				return;
			}

			if (other.IsBodyCollider() && other.GetUnit() == _target)
			{
				_isTargetInCombatRange = false;
				OnTargetInRangeChanged?.Invoke();
			}
		}
	}
}