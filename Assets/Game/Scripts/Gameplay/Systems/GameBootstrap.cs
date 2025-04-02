using VContainer;
using VContainer.Unity;

namespace ForestRoyale.Gameplay.Systems
{
	public class GameBootstrap : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			builder.Register<ArenaEvents>(Lifetime.Scoped);
			builder.Register<MovementSystem>(Lifetime.Scoped);
			builder.Register<TargetingSystem>(Lifetime.Scoped);
		}
	}
}