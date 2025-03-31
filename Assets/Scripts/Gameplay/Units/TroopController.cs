using UnityEngine;
using UnityEngine.AI;
using ForestRoyale.Gameplay.Units;
using ForestRoyale.Gameplay.Cards;
using Raven.Attributes;

namespace ForestRoyale.Gameplay.Navigation
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

		[SerializeField] private Transform _target;

		private Collider[] GetOverlappingColliders()
		{
			// Get all colliders that are currently overlapping with the attack collider
			Collider[] overlappingColliders = Physics.OverlapSphere(
				_attackCollider.bounds.center,
				_attackCollider.bounds.extents.magnitude,
				LayerMask.GetMask("Default")
			);

			return overlappingColliders;
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

			_agent.SetDestination(_target.position);
		}

		void Update()
		{
			foreach (Collider collider in GetOverlappingColliders())
			{
				if (collider.TryGetComponent<TroopController>(out TroopController troopController))
				{
					//stop
					_agent.isStopped = true;

					// TODO: Attack the target
				}
			}
		}
	}
}