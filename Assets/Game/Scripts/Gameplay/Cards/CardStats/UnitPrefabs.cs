using ForestRoyale.Core.Pool;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ForestRoyale.Gameplay.Cards.CardStats
{
	[Serializable]
	public class UnitPrefabs
	{
		[SerializeField, Required]
		[BoxGroup("Troop")]
		private PooledGameObject _unitPrefab = null;

		[SerializeField]
		[BoxGroup("Troop")]
		private PooledGameObject _projectilePrefab = null;

		public PooledGameObject UnitPrefab => _unitPrefab;
		public PooledGameObject ProjectilePrefab => _projectilePrefab;

		public static UnitPrefabs Build(PooledGameObject unitPrefab, PooledGameObject projectilePrefab)
		{
			return new UnitPrefabs()
			{
				_unitPrefab = unitPrefab,
				_projectilePrefab = projectilePrefab
			};
		}
	}
}