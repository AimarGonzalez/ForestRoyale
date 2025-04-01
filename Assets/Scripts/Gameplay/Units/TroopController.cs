using UnityEngine;
using UnityEngine.AI;
using ForestRoyale.Gameplay.Units;
using ForestRoyale.Gameplay.Cards;
using Raven.Attributes;
using UnityEditor;

namespace ForestRoyale.Gameplay.Units
{
	public class TroopController : MonoBehaviour
	{
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

		[SerializeField] private TroopController _target;

		private bool _isInCombatRange;

		public Vector3 Position => transform.position;

		public TroopController()
		{
		}

		public TroopData TroopData => _troopData;

		private void Awake()
		{
			if (_attackCollider != null)
			{
				var trigger = _attackCollider.gameObject.AddComponent<TriggerListener>();
				trigger.OnTriggerEnterEvent += HandleTriggerEnter;
				trigger.OnTriggerExitEvent += HandleTriggerExit;
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
				}
			}
		}

		private void HandleTriggerEnter(Collider other)
		{
			if (other.IsBodyCollider() && other.GetTroopController() == _target)
			{
				_isInCombatRange = true;
				OnTargetInRange();
			}
		}

		private void HandleTriggerExit(Collider other)
		{
			if (other.IsBodyCollider() && other.GetTroopController() == _target)
			{
				_isInCombatRange = false;
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
			if (!_target)
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

			_agent.SetDestination(_target.Position);
		}

		void Update()
		{
		}
		
		private void OnTargetInRange()
		{
			_agent.isStopped = true;
		}
	}

	public class TriggerListener : MonoBehaviour
	{
		public event System.Action<Collider> OnTriggerEnterEvent;
		public event System.Action<Collider> OnTriggerExitEvent;

		private void OnTriggerEnter(Collider other)
		{
			OnTriggerEnterEvent?.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			OnTriggerExitEvent?.Invoke(other);
		}
	}
}