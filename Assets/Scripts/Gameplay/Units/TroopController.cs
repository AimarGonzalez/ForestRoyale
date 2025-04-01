using UnityEngine;
using UnityEngine.AI;
using ForestRoyale.Gameplay.Units;
using ForestRoyale.Gameplay.Cards;
using Raven.Attributes;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;

namespace ForestRoyale.Gameplay.Units
{
	public class TroopController : MonoBehaviour
	{
		private class TargetData
		{
			public TroopController Troop;
			public bool IsInCombatRange = false;

			public TargetData(TroopController troop)
			{
				Troop = troop;
			}
		}
		
		[SerializeField]
		[Required]
		private NavMeshAgent _agent;

		[SerializeField]
		[Required]
		private TroopCardData _cardData;

		[SerializeField]
		[Required]
		private Collider _bodyCollider;

		[SerializeField]
		[Required]
		private Collider _attackCollider;

		[ShowInInspector] private TroopData _troopData;

		[SerializeField] private TroopController _debugTarget;

		private TargetData _target = null;
		
		public Vector3 Position => transform.position;

		public TroopData TroopData => _troopData;
		public TroopController Target => _target.Troop;

		private void Awake()
		{
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

		private void HandleTriggerEnter(Collider other)
		{
			if (_target == null)
			{
				return;
			}
			
			if (other.IsBodyCollider() && other.GetTroopController() == _target.Troop)
			{
				_target.IsInCombatRange = true;
				OnTargetInAttackRange();
			}
		}

		private void HandleTriggerStay(Collider other)
		{
			if (_target == null)
			{
				return;
			}
			
			if (other.IsBodyCollider() && other.GetTroopController() == _target.Troop)
			{
				_target.IsInCombatRange = true;
				OnTargetInAttackRange();
			}
		}

		private void HandleTriggerExit(Collider other)
		{
			if (_target == null)
			{
				return;
			}
			
			if (other.IsBodyCollider() && other.GetTroopController() == _target.Troop)
			{
				_target.IsInCombatRange = false;
				OnTargetOutOfAttackRange();
			}
		}

		void OnValidate()
		{
			if (_cardData != null)
			{
				_troopData = new TroopData(_cardData);
			}
		}

		[Button]
		void GoToTarget()
		{
			if (!_debugTarget)
			{
				return;
			}

			if (!_agent)
			{
				EditorUtility.DisplayDialog("Error", "Agent is not assigned", "OK");
				return;
			}

			if (!_agent.isOnNavMesh)
			{
				EditorUtility.DisplayDialog("Error", "Agent is not on NavMesh", "OK");
				return;
			}

			SetTarget(_debugTarget);
		}

		public void SetTarget(TroopController troop)
		{
			_target = new TargetData(troop);
			
			// Resumes agent movement
			_agent.isStopped = false;
			_agent.destination = _target.Troop.Position;
		}

		private void OnTargetInAttackRange()
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