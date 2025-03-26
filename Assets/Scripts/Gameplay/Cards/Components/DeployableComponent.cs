using Raven.Attributes;
using System;
using UnityEngine;

namespace ForestRoyale.Gameplay.Cards.Components
{
	[Serializable]
	public class DeployableComponent
	{
		[BoxGroup("Troop Properties")]
		[Tooltip("Deployment time in seconds")]
		[SerializeField]
		private float _deploymentTime = 1.0f;

		public float DeploymentTime => _deploymentTime;
	}
}