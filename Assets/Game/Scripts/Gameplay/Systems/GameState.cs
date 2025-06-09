using ForestRoyale.Core.UI;
using ForestRoyale.Gameplay.Systems;
using UnityEngine;
using VContainer;


namespace ForestRoyale.Gameplay.Combat
{
	public class GameState : MonoBehaviour, IGUIDrawer
	{
		[SerializeField] private DeckDataSO _playerDeck;
		[SerializeField] private DeckDataSO _botDeck;
		[SerializeField] private float _battleDuration = 180f; // 3 minutes


		[Inject]
		private Arena _arena;

		[Inject]
		private ApplicationEvents _appEvents;


		private Battle _battle;

		public bool HasActiveBattle => _battle != null && _battle.IsBattleActive;

		private void Start()
		{
			InitializeBattle();
		}

		private void InitializeBattle()
		{
			_battle = new Battle(_battleDuration);
			_battle.Player.Deck.Initialize(_playerDeck.Cards);
			_battle.Bot.Deck.Initialize(_botDeck.Cards);

			_battle.ResetBattle();
			_appEvents.TriggerBattleCreated(_battle);
		}

		private void StartBattle()
		{
			_battle.StartBattle();
			_appEvents.TriggerBattleStarted(_battle);
		}

		public void EndBattle()
		{
			_battle.EndBattle();
			_appEvents.TriggerBattleEnded(_battle);
		}

		public void ResetBattle()
		{
			_battle.ResetBattle();
			_appEvents.TriggerBattleCreated(_battle);
		}

		public void DrawGUI()
		{
			GUILayoutUtils.Label("Battle");
			GUILayoutUtils.Label($"Remaining Time: {_battle.RemainingTime}");

			GUILayout.BeginHorizontal();


			if (GUILayout.Button("Start Battle"))
			{
				StartBattle();
			}

			if (GUILayout.Button("End Battle"))
			{
				EndBattle();
			}

			if (GUILayout.Button("Reset Battle"))
			{
				ResetBattle();
			}

			GUILayout.EndHorizontal();

			if (GUILayout.Button("Respawn towers"))
			{
				_arena.ResetTowers();
			}
		}
	}
}