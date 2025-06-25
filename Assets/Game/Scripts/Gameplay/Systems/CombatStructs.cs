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


	public class ProjectileData
	{
		private PooledGameObject _pooledGameObject;
		private Unit _attacker;
		private Unit _target;
		private Transform _transform;
		private Vector3 _position;
		private float _speed;

		public PooledGameObject PooledGameObject => _pooledGameObject;
		public Unit Attacker => _attacker;
		public Unit Target => _target;
		public Vector3 Position
		{
			get => _position;
			set
			{
				_position = value;
				_transform.position = value;
			}
		}

		public Quaternion Rotation
		{
			get => _transform.rotation;
			set
			{
				_transform.rotation = value;
			}
		}

		public float Speed => _speed;

		public void Init(Unit attacker, Unit target, PooledGameObject pooledGameObject)
		{
			_pooledGameObject = pooledGameObject;
			_attacker = attacker;
			_target = target;
			_speed = attacker.CombatStats.ProjectileSpeed;
			_transform = pooledGameObject.transform;
			_position = _transform.position;
		}
	}
}