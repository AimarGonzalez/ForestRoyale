using ForestRoyale.Core.Pool;
using ForestRoyale.Gameplay.Combat;
using ForestRoyale.Gameplay.Settings;
using ForestRoyale.Gameplay.Units;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ForestRoyale.Gameplay.Systems
{
	public class ProjectilesSystem
	{
		// ----- Dependencies ----------
		private readonly ArenaEvents _arenaEvents;
		private readonly ProjectileViewFactory _projectileViewFactory;
		private readonly CombatSettings _combatSettings;


		// ----- Data ------------------
		private List<ProjectileData> _projectiles = new();

		private readonly ObjectPool<ProjectileData> _projectilePool = new(
			() => new ProjectileData(),
			null,
			null,
			null,
			collectionCheck: false,
			maxSize: 100,
			defaultCapacity: 10
		);

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

			PooledGameObject projectileView = _projectileViewFactory.BuildProjectile(attacker);
			ProjectileData projectileData = GetFromPool(attacker, target, projectileView);

			MoveToTarget(projectileData, _combatSettings.Projectile.InitialOffset);

			_projectiles.Add(projectileData);
		}

		private ProjectileData GetFromPool(Unit attacker, Unit target, PooledGameObject projectileInstance)
		{
			ProjectileData projectileData = _projectilePool.Get();
			projectileData.Init(attacker, target, projectileInstance);
			return projectileData;
		}

		private void ReleaseToPool(ProjectileData projectileData)
		{
			projectileData.PooledGameObject.ReleaseToPool();
			_projectiles.Remove(projectileData);
			_projectilePool.Release(projectileData);
		}

		private void HandleUnitRemoved(Unit unit)
		{
			for (int i = _projectiles.Count - 1; i >= 0; i--)
			{
				if (_projectiles[i].Target == unit)
				{
					ReleaseToPool(_projectiles[i]);
				}
			}
		}

		public void UpdateProjectiles()
		{
			for (int i = _projectiles.Count - 1; i >= 0; i--)
			{
				ProjectileData projectileData = _projectiles[i];
				float distanceToTarget = MoveToTarget(projectileData);
				ProcessHit(projectileData, distanceToTarget);
			}
		}

		private float MoveToTarget(ProjectileData projectile)
		{
			Vector3 targetPos = projectile.Target.Body.GetTargetPositionFrom(projectile.Position);

			Vector3 direction = targetPos - projectile.Position;
			float distanceToTarget = direction.magnitude;
			float distanceToMove = Mathf.Min(distanceToTarget, projectile.Speed * Time.deltaTime);
			Vector3 newPosition = projectile.Position + direction.normalized * distanceToMove;

			projectile.TargetPosition = targetPos;
			projectile.Position = newPosition;

			projectile.Rotation = Quaternion.LookRotation(direction, Vector3.up);

			return distanceToTarget;
		}

		private bool IsTargetHit(float distanceToTarget)
		{
			return distanceToTarget < _combatSettings.Projectile.HitDistance;
		}

		private float MoveToTarget(ProjectileData projectile, float distance)
		{
			Vector3 targetPos = projectile.Target.Body.GetTargetPositionFrom(projectile.Position);
			Vector3 direction = targetPos - projectile.Position;
			float distanceToTarget = direction.magnitude;
			float distanceToMove = Mathf.Min(distanceToTarget, distance);
			Vector3 newPosition = projectile.Position + direction.normalized * distanceToMove;

			projectile.TargetPosition = targetPos;
			projectile.Position = newPosition;

			projectile.Rotation = Quaternion.LookRotation(direction, Vector3.up);

			return distanceToTarget;
		}

		private void ProcessHit(ProjectileData projectile, float distanceToTarget)
		{
			if (IsTargetHit(distanceToTarget))
			{
				_arenaEvents.TriggerProjectileHit(projectile.Attacker, projectile.Target);
				ReleaseToPool(projectile);
			}
		}

		public void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			foreach (ProjectileData projectile in _projectiles)
			{
				Gizmos.DrawLine(projectile.Position, projectile.TargetPosition);
				Gizmos.DrawWireSphere(projectile.TargetPosition, 0.1f);
			}
		}
	}
}