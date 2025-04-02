namespace ForestRoyale.Gameplay.Units
{
	public interface IDamageable
	{
		float Health { get; }
		float MaxHealth { get; }
		float HealthRatio => Health / MaxHealth;
	}
}
