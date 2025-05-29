using ForestRoyale.Core;
using ForestRoyale.Gameplay.Combat;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ForestRoyale.Gameplay.Systems
{
	[DefaultExecutionOrder(-5000)]
	public class GameBootstrap : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			Debug.Log("Boostrap");
			builder.Register<ApplicationEvents>(Lifetime.Singleton);
			builder.Register<ArenaSystemsLoop>(Lifetime.Scoped);
			builder.Register<ArenaEvents>(Lifetime.Scoped);
			builder.Register<MovementSystem>(Lifetime.Scoped);
			builder.Register<TargetingSystem>(Lifetime.Scoped);
			builder.Register<CombatSystem>(Lifetime.Scoped);
			builder.Register<UnitStateMachine>(Lifetime.Scoped);

			// Register MonoBehaviours
			builder.UseComponents(components =>
			{
				components.AddInHierarchy<TimeController>();
				components.AddInHierarchy<CardCaster>();
				components.AddInHierarchy<CardCastingViewFactory>();
				components.AddInHierarchy<Battle>();
			});
		}
	}
}