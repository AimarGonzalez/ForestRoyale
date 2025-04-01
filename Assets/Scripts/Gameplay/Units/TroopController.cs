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

		private void OnTriggerEnter(Collider other)
		{
			if (other.IsBodyCollider() && other.GetTroopController() == _target)
			{
				_isInCombatRange = true;
			}
		}

		private void OnTriggerExit(Collider other)
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
				//show error message window
				EditorUtility.DisplayDialog("Error", "Agent is not assigned", "OK");
				return;
			}

			if (!_agent.isOnNavMesh)
			{
				//show error message window
				EditorUtility.DisplayDialog("Error", "Agent is not on NavMesh", "OK");
				return;
			}

			_agent.SetDestination(_target.Position);
		}

		void Update()
		{
			if (_isInCombatRange)
			{
				//stop
				_agent.isStopped = true;

				// TODO: Attack the target
			}
		}
	}
}