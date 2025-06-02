namespace ForestRoyale.Gameplay.Units.MonoBehaviours
{
	public interface IUnitChangeListener
	{
		void OnUnitChanged(Unit oldUnit, Unit newUnit);
	}
}
