using ForestRoyale.Gameplay.Combat;
using ForestRoyale.Gameplay.Systems;
using ForestRoyale.Gameplay.Units.MonoBehaviours.Components;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace ForestRoyale.Gameplay.UI
{
	public class GameStateUIView : UIView<GameState.State>
	{
		[Inject]
		private ApplicationEvents _appEvents;

		private void Awake()
		{
			_appEvents.OnGameStateChanged += OnGameStateChanged;
		}

		private void OnDestroy()
		{
			_appEvents.OnGameStateChanged -= OnGameStateChanged;
		}

		private void OnGameStateChanged(GameState.State oldState, GameState.State newState)
		{
			base.OnStateChanged(oldState, newState);
		}
	}
}
