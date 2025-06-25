using ForestRoyale.Gameplay.Units;
using UnityEngine;
using System.Collections.Generic;

namespace ForestRoyale.Gameplay.Systems
{
	public class ProjectilesSystem
	{
		// ----- Dependencies ----------
		private readonly ArenaEvents _arenaEvents;

		// ----- Data ------------------
		private List<ProjectileData> _projectiles = new List<ProjectileData>();

		public ProjectilesSystem(ArenaEvents arenaEvents)
		{
			_arenaEvents = arenaEvents;

			_arenaEvents.OnProjectileFired += HandleProjectileFired;
		}

		private void HandleProjectileFired(Unit attacker, Unit target)
		{
			Debug.Log($"Projectile fired from {attacker.Id} to {target.Id}");

			// Get reference to projectile prefab

			// Create projectile instance

			// Create ProjectileData and store it in the list
		}


		private void UpdateProjectiles()
		{
			foreach (ProjectileData projectile in _projectiles)
			{
				// Move projectile in the direction of the target

				// Rotate projectile to face the target
			}
		}
	}
}