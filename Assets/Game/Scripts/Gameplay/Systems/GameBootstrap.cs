using ForestRoyale.Gameplay.Units.MonoBehaviors;
using VContainer;
using VContainer.Unity;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace ForestRoyale.Gameplay.Systems
{
	[DefaultExecutionOrder(-5000)]
	public class GameBootstrap : LifetimeScope
	{



		protected override void Configure(IContainerBuilder builder)
		{
			Debug.Log("Boostrap");
			builder.Register<ArenaSystemsLoop>(Lifetime.Scoped);
			builder.Register<ArenaEvents>(Lifetime.Scoped);
			builder.Register<MovementSystem>(Lifetime.Scoped);
			builder.Register<TargetingSystem>(Lifetime.Scoped);
		}
	}
}