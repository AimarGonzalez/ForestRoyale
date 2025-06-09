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
		private ArenaEvents _arenaEvents;
		

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
			
			_arenaEvents.TriggerBattleCreated(_battle);
		}

		public void DrawGUI()
		{
			GUILayoutUtils.Label("Battle");
			GUILayoutUtils.Label($"Remaining Time: {_battle.RemainingTime}");

			GUILayout.BeginHorizontal();


			if (GUILayout.Button("Start Battle"))
			{
				_battle.StartBattle();
			}

			if (GUILayout.Button("End Battle"))
			{
				_battle.EndBattle();
			}
			
			if (GUILayout.Button("Reset Battle"))
			{
				_battle.ResetBattle();
				_arenaEvents.TriggerBattleCreated(_battle);
			}

			GUILayout.EndHorizontal();

			if (GUILayout.Button("Respawn towers"))
			{
				_arena.ResetTowers();
			}
		}
	}
}