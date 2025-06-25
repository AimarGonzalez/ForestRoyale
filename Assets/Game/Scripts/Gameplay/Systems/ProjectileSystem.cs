using ForestRoyale.Core.Pool;
using ForestRoyale.Gameplay.Combat;
using ForestRoyale.Gameplay.Settings;
using ForestRoyale.Gameplay.Units;
using System.Collections.Generic;
using UnityEngine;

namespace ForestRoyale.Gameplay.Systems
{
	public class ProjectilesSystem
	{
		// ----- Dependencies ----------
		private readonly ArenaEvents _arenaEvents;
		private readonly ProjectileViewFactory _projectileViewFactory;
		private readonly CombatSettings _combatSettings;
		

		// ----- Data ------------------
		private List<ProjectileData> _projectiles = new ();

		public ProjectilesSystem(ArenaEvents arenaEvents, ProjectileViewFactory projectileViewFactory, GameSettings gameSettings)
		{
			_arenaEvents = arenaEvents;
			_projectileViewFactory = projectileViewFactory;
			_combatSettings = gameSettings.CombatSettings;

			_arenaEvents.OnProjectileFired += HandleProjectileFired;
			_arenaEvents.OnUnitRemoved += HandleUnitRemoved;
		}

		private void HandleProjectileFired(Unit attacker, Unit target)
		{
			Debug.Log($"Projectile fired from {attacker.Id} to {target.Id}");

			// Get reference to projectile prefab
			PooledGameObject projectileInstance = _projectileViewFactory.BuildProjectile(attacker);
			ProjectileData projectileData = new ProjectileData(attacker, target, attacker.CombatStats.ProjectileSpeed, projectileInstance);

			// Orient projectile to face the target
			MoveTowardsTarget(projectileData, 1f);

			// Create ProjectileData and store it in the list
			_projectiles.Add(projectileData);
		}

		private void HandleUnitRemoved(Unit unit)
		{
			_projectiles.RemoveAll(p => p.Target == unit);
		}

		private void UpdateProjectiles()
		{
			foreach (ProjectileData projectileData in _projectiles)
			{
				if (MoveTowardsTarget(projectileData))
				{
					_projectiles.Remove(projectileData);
					_arenaEvents.TriggerProjectileHit(projectileData.Attacker, projectileData.Target);
				}
			}
		}

		private bool MoveTowardsTarget(ProjectileData projectile)
		{
			Vector3 targetPos = projectile.Target.UnitRoot.transform.position;

			Vector3 direction = targetPos - projectile.Position;
			float distanceToTarget = direction.magnitude;
			float distanceToMove = Mathf.Min(distanceToTarget, projectile.Speed * Time.deltaTime);
			Vector3 newPosition = projectile.Position + direction.normalized * distanceToMove;

			projectile.Transform.position = newPosition;
			projectile.Position = newPosition;

			projectile.Transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

			return IsTargetHit(distanceToTarget);
		}

		private bool IsTargetHit(float distanceToTarget)
		{
			return distanceToTarget < _combatSettings.HitDistance;
		}

		private void MoveTowardsTarget(ProjectileData projectile, float distance)
		{
			Vector3 targetPos = projectile.Target.UnitRoot.transform.position;
			Vector3 direction = targetPos - projectile.Position;
			float distanceToTarget = direction.magnitude;
			float distanceToMove = Mathf.Min(distanceToTarget, distance);
			Vector3 newPosition = projectile.Position + direction.normalized * distanceToMove;

			projectile.Transform.position = newPosition;
			projectile.Position = newPosition;

			projectile.Transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
		}
	}
}