using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace ForestRoyale.Gameplay.Units.MonoBehaviors
{
	public class MovementController : MonoBehaviour
	{
		private class TargetData
		{
			public UnitRoot UnitRoot;
			public Unit Unit;
			public bool IsInCombatRange = false;
		}

		[SerializeField]
		private UnitRoot _root;

		[SerializeField]
		[Required]
		private NavMeshAgent _agent;

		[SerializeField]
		[Required]
		private Collider _bodyCollider;

		[SerializeField]
		[Required]
		private CapsuleCollider _attackCollider;


		[ShowInInspector, ReadOnly]
		private Unit _unit;

		public Unit Unit => _unit;

		private void Awake()
		{
			_root ??= GetComponent<UnitRoot>();

			_root.OnUnitChanged += OnUnitChanged;

			if (_attackCollider != null)
			{
				var trigger = _attackCollider.gameObject.AddComponent<TriggerListener>();
				trigger.OnTriggerEnterEvent += HandleTriggerEnter;
				trigger.OnTriggerExitEvent += HandleTriggerExit;
				trigger.OnTriggerStayEvent += HandleTriggerStay;
			}
		}

		private void OnDestroy()
		{
			_root.OnUnitChanged -= OnUnitChanged;

			if (_attackCollider != null)
			{
				var trigger = _attackCollider.GetComponent<TriggerListener>();
				if (trigger != null)
				{
					trigger.OnTriggerEnterEvent -= HandleTriggerEnter;
					trigger.OnTriggerExitEvent -= HandleTriggerExit;
					trigger.OnTriggerStayEvent -= HandleTriggerStay;
				}
			}
		}

		private void OnUnitChanged(Unit newUnit)
		{
			_unit = newUnit;

			if (_unit == null)
			{
				return;
			}

			if (_agent == null)
			{
				// This unit cannot move (e.g. buildings)
				return;
			}

			// Update agent with unit stats
			_agent.speed = _unit.UnitStats.MovementSpeed;

			// Update attackCollider with unit stats
			_attackCollider.radius = _unit.CombatStats.AttackRange;
		}


		private void HandleTriggerEnter(Collider other)
		{
			if (_unit?.Target == null)
			{
				return;
			}

			if (other.IsBodyCollider() && other.GetUnit() == _unit.Target)
			{
				_unit.TargetIsInCombatRange = true;
				OnTargetInCombatRange();
			}
		}

		private void HandleTriggerStay(Collider other)
		{
			if (_unit?.Target == null)
			{
				return;
			}

			if (other.IsBodyCollider() && other.GetUnit() == _unit.Target)
			{
				_unit.TargetIsInCombatRange = true;
				OnTargetInCombatRange();
			}
		}

		private void HandleTriggerExit(Collider other)
		{
			if (_unit?.Target == null)
			{
				return;
			}

			if (other.IsBodyCollider() && other.GetUnit() == _unit.Target)
			{
				_unit.TargetIsInCombatRange = false;
				OnTargetOutOfAttackRange();
			}
		}

		public void MoveToTarget()
		{
			// Resumes agent movement

			Assert.NotNull(_unit.Target);
			_agent.isStopped = false;
			_agent.destination = _unit.Target.Position;
		}

		private void OnTargetInCombatRange()
		{
			// Stops agent movement
			_agent.isStopped = true;
		}

		private void OnTargetOutOfAttackRange()
		{
			// Resumes agent movement
			_agent.isStopped = false;
		}
	}
}