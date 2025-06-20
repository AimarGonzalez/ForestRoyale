using ForestRoyale.Core.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class AttackRangeMonitor : UnitComponent, IUnitChangeListener
	{
		[SerializeField]
		[Required]
		private Collider2D _bodyCollider;

		[SerializeField]
		[Required]
		private CircleCollider2D _attackCollider;

		[ShowInInspector]
		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private Unit _target;

		[ShowInInspector]
		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		private bool _isTargetInCombatRange = false;

		private Collider2DListener _attackColliderListener;

		private bool _isDetectionEnabled = false;
		private bool _isSubscribed = false;

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
				_attackColliderListener = _attackCollider.GetComponent<Collider2DListener>();
			}
		}

		public void SetDetectionEnabled(bool enabled)
		{
			if (_isDetectionEnabled == enabled)
			{
				return;
			}

			_isDetectionEnabled = enabled;

			UpdateDetectionState();
		}

		void IUnitChangeListener.OnUnitChanged(Unit oldUnit, Unit newUnit)
		{
			if (Unit != null)
			{
				_attackCollider.radius = Unit.CombatStats.AttackRange;
			}
			else
			{
				_attackCollider.radius = 0f;
			}

			UpdateDetectionState();
		}

		private void UpdateDetectionState()
		{
			if (_isDetectionEnabled && Unit != null)
			{
				Subscribe();
			}
			else
			{
				Unsubscribe();
			}
		}

		private void Subscribe()
		{
			if (_isSubscribed)
			{
				return;
			}

			_isSubscribed = true;

			if (_attackColliderListener != null)
			{
				_attackCollider.enabled = true;
				_attackColliderListener.OnTriggerEnterEvent += HandleTriggerEnter;
				_attackColliderListener.OnTriggerExitEvent += HandleTriggerExit;
				_attackColliderListener.OnTriggerStayEvent += HandleTriggerStay;
			}
		}

		private void Unsubscribe()
		{
			if (!_isSubscribed)
			{
				return;
			}

			_isSubscribed = false;

			if (_attackColliderListener != null)
			{
				_attackCollider.enabled = false;
				_attackColliderListener.OnTriggerEnterEvent -= HandleTriggerEnter;
				_attackColliderListener.OnTriggerExitEvent -= HandleTriggerExit;
				_attackColliderListener.OnTriggerStayEvent -= HandleTriggerStay;
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
			if (!_isTargetInCombatRange)
			{
				return;
			}

			if (_target == null)
			{
				return;
			}

			if (other == _bodyCollider)
			{
				// ignore my own colliders
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