using UnityEngine;
namespace ForestRoyale.Gameplay.Units.MonoBehaviours.Components
{
	public class TowerDeathBehavior : MonoBehaviour, IDeathBehavior
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
