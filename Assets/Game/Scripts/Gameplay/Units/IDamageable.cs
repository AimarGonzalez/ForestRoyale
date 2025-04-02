namespace ForestRoyale.Gameplay.Units
{
	public interface IDamageable
	{
		float CurrentHealth { get; }
		float MaxHealth { get; }
		float HealthRatio => CurrentHealth / MaxHealth;
	}
}
