using ForestRoyale.Gameplay.Units;
using ForestRoyale.Gameplay.Units.MonoBehaviors;
using ForestRoyale.Gui;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ForestRoyale.Game.Scripts.Gameplay.Units.MonoBehaviours
{
	public class AttackRangeMonitor : UnitComponent
	{
		[SerializeField]
		[Required]
		private CapsuleCollider _attackCollider;

		[ShowInInspector]
		[BoxGroup(InspectorConstants.DebugBoxGroup), PropertyOrder(InspectorConstants.DebugBoxGroupOrder)]
		private string Target => Unit?.Target?.Id ?? "<none>";
		
		[ShowInInspector]
		[BoxGroup(InspectorConstants.DebugBoxGroup), PropertyOrder(InspectorConstants.DebugBoxGroupOrder)]
		private bool IsTargetInCombatRange => Unit?.TargetIsInCombatRange ?? false;


		private TriggerListener _targetListener;


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
			if (Unit?.Target == null)
			{
				return;
			}

			if (other.IsBodyCollider() && other.GetUnit() == Unit.Target)
			{
				Unit.TargetIsInCombatRange = true;
				OnTargetInRangeChanged?.Invoke();
			}
		}

		private void HandleTriggerStay(Collider other)
		{
			if (Unit?.Target == null)
			{
				return;
			}

			if (other.IsBodyCollider() && other.GetUnit() == Unit.Target)
			{
				Unit.TargetIsInCombatRange = true;
				OnTargetInRangeChanged?.Invoke();
			}
		}

		private void HandleTriggerExit(Collider other)
		{
			if (Unit?.Target == null)
			{
				return;
			}

			if (other.IsBodyCollider() && other.GetUnit() == Unit.Target)
			{
				Unit.TargetIsInCombatRange = false;
				OnTargetInRangeChanged?.Invoke();
			}
		}

		
	}
	
	
}