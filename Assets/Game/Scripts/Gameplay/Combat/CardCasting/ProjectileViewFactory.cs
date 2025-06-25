using ForestRoyale.Core.Pool;
using ForestRoyale.Gameplay.Units;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace ForestRoyale.Gameplay.Combat
{
	public class ProjectileViewFactory : MonoBehaviour
	{
		[SerializeField, Required]
		private Transform _projectilesContainer;
		
		[Inject]
		private IObjectResolver _container;
		
		[Inject]
		private GameObjectPoolService _poolService;

		public PooledGameObject BuildProjectile(Unit attacker)
		{
			PooledGameObject projectile = _poolService.Get(attacker.Prefabs.ProjectilePrefab);
			projectile.transform.SetParent(_projectilesContainer);
			return projectile;
		}
	}
}
