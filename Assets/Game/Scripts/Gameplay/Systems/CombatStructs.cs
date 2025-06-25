using System.Collections.Generic;
using ForestRoyale.Gameplay.Units;
using ForestRoyale.Core.Pool;
using UnityEngine;

namespace ForestRoyale.Gameplay.Systems
{
	public struct HitData
	{
		public Unit Attacker;
		public List<Unit> Targets;
		public float Damage;

		public HitData(Unit attacker, List<Unit> targets, float damage)
		{
			Attacker = attacker;
			Targets = targets;
			Damage = damage;
		}
	}


	public struct ProjectileData
	{
		public Unit Attacker;
		public Unit Target;
		public float Speed;
		public PooledGameObject PooledGameObject;
		public Transform Transform;
		public Vector3 Position;

		public ProjectileData(Unit attacker, Unit target, float speed, PooledGameObject pooledGameObject)
		{
			Attacker = attacker;
			Target = target;
			Speed = speed;
			PooledGameObject = pooledGameObject;
			Transform = pooledGameObject.transform;
			Position = Transform.position;
		}
	}
}