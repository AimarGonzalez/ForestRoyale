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
			_aliveRoot.SetActive(true);
			_deadRoot.SetActive(false);
		}

		public void OnDeath()
		{
			_aliveRoot.SetActive(false);
			_deadRoot.SetActive(true);
		}
		
		void IReseteable.Reset()
		{
			_aliveRoot.SetActive(true);
			_deadRoot.SetActive(false);
		}
	}
}
