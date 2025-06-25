using System.Collections.Generic;
using ForestRoyale.Gameplay.Units;
using ForestRoyale.Core.Pool;

namespace ForestRoyale.Gameplay.Systems
{
	public struct HitData
	{
		public Unit Attacker;
		public List<Unit> Targets;
		public float Damage;
	}


	public struct ProjectileData
	{
		public Unit Attacker;
		public Unit Target;
		public float Speed;
		public PooledGameObject PooledGameObject;
	}
}