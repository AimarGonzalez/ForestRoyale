using ForestRoyale.Gameplay.Combat;
using ForestRoyale.Gameplay.Systems;
using ForestRoyale.Gameplay.Units.MonoBehaviours.Components;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace ForestRoyale.Gameplay.UI
{
	public class GameStateUIInput : MonoBehaviour
	{
		[Inject]
		private GameState _gameState;

		[SerializeField]
		private Button _startBannerButton;

		private void Awake()
		{
			_startBannerButton.onClick.AddListener(OnStartBannerButtonClicked);
		}

		private void OnStartBannerButtonClicked()
		{
			_gameState.StartBattle();
		}
	}
}
