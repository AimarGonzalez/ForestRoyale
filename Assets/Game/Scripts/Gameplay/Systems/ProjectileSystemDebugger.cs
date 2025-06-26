using UnityEngine;
using VContainer;

namespace ForestRoyale.Gameplay.Systems
{
	public class ProjectileSystemDebugger : MonoBehaviour
	{
		[Inject]
		private ProjectilesSystem _projectilesSystem;

		public void OnDrawGizmos()
		{
			_projectilesSystem?.OnDrawGizmos();
		}
	}
}