using ForestRoyale.Gameplay.Units.MonoBehaviors;
using Raven.Attributes;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Gameplay.Units.MonoBehaviours
{
	[RequireComponent(typeof(UnitRoot))]
	public class UnitDebugTools : MonoBehaviour
	{

		[SerializeField]
		private UnitRoot _target;

		private UnitRoot _root;

		[Inject]
		private TargetingSystem _targetingSystem;

		private void Awake()
		{
			_root = GetComponent<UnitRoot>();
		}

		[Button]
		void GoToTarget()
		{
			if (!_target)
			{
				return;
			}

			_targetingSystem.SetTarget(_root.Unit, _target.Unit);
		}
	}
}