using ForestRoyale.Core;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{

	// DEPRECATED: Use UnitView instead
	[Obsolete("DEPRECATED: Use UnitView instead")]
	public class DeathComponent : MonoBehaviour, IDeathComponent, IReseteable
	{
		[InfoBox("DEPRECATED: Use UnitView instead", InfoMessageType.Warning)]
		[SerializeField]
		private GameObject _aliveRoot;

		[SerializeField]
		private GameObject _deadRoot;

		private void Start()
		{
			_deadRoot.SetActive(false);
			_aliveRoot.SetActive(true);
		}

		public void OnDeath()
		{
			_aliveRoot.SetActive(false);
			_deadRoot.SetActive(true);
		}

		void IReseteable.Reset()
		{
			_deadRoot.SetActive(false);
			_aliveRoot.SetActive(true);
		}
	}
}
