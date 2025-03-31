namespace ForestRoyale.Gameplay.Units
{
	public interface IDamageable
	{
		float RemainingHealth { get; }
		float MaxHealth { get; }
		float HealthRatio => RemainingHealth / MaxHealth;
	}
}
