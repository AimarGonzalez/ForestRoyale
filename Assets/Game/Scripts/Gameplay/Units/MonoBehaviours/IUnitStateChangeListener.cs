namespace ForestRoyale.Gameplay.Units.MonoBehaviours
{
	public interface IUnitStateChangeListener
	{
		void OnUnitStateChanged(UnitState oldState, UnitState newState);
	}
}
