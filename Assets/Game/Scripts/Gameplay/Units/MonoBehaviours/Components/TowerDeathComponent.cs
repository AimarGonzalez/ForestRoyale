using UnityEngine;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class TowerDeathComponent : MonoBehaviour, IDeathComponent
	{
		[SerializeField]
		private GameObject _aliveRoot;

		[SerializeField]
		private GameObject _deadRoot;

		public void OnDeath()
		{
			_aliveRoot.SetActive(false);
			_deadRoot.SetActive(true);
		}
	}
}
