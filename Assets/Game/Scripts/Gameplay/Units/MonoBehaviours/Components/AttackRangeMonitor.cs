using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class AttackRangeMonitor : UnitComponent
	{
		[SerializeField]
		[Required]
		private Collider2D _bodyCollider;
		
		[SerializeField]
		[Required]
		private CircleCollider2D _attackCollider;

		[ShowInInspector]
		[BoxGroup(InspectorConstants.DebugGroup), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		private Unit _target;

		[ShowInInspector]
		[BoxGroup(InspectorConstants.DebugGroup), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		private bool _isTargetInCombatRange = false;

		private Collider2DListener _targetListener;

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
				_targetListener = _attackCollider.GetComponent<Collider2DListener>();
				_targetListener.OnTriggerEnterEvent += HandleTriggerEnter;
				_targetListener.OnTriggerExitEvent += HandleTriggerExit;
				_targetListener.OnTriggerStayEvent += HandleTriggerStay;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (_targetListener != null)
			{
				_targetListener.OnTriggerEnterEvent -= HandleTriggerEnter;
				_targetListener.OnTriggerExitEvent -= HandleTriggerExit;
				_targetListener.OnTriggerStayEvent -= HandleTriggerStay;
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

		private void HandleTriggerEnter(Collider2D other)
		{
			if (_target == null)
			{
				return;
			}

			if (other == _bodyCollider)
			{
				// ignore own colliders
				return;
			}

			if (other.IsBodyCollider() && other.GetUnit() == _target)
			{
				_isTargetInCombatRange = true;
				OnTargetInRangeChanged?.Invoke();
			}
		}

		private void HandleTriggerStay(Collider2D other)
		{
			if (_target == null)
			{
				return;
			}

			if (other == _bodyCollider)
			{
				// ignore own colliders
				return;
			}
			
			if (other.IsBodyCollider() && other.GetUnit() == _target)
			{
				_isTargetInCombatRange = true;
				OnTargetInRangeChanged?.Invoke();
			}
		}

		private void HandleTriggerExit(Collider2D other)
		{
			if (_target == null)
			{
				return;
			}
			
			if (other == _bodyCollider)
			{
				// ignore own colliders
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