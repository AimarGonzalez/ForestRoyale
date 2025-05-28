using ForestRoyale.Core;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class DeathComponent : MonoBehaviour, IDeathComponent, IReseteable
	{
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
