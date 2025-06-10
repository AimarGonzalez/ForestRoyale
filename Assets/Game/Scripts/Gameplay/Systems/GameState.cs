using ForestRoyale.Core.UI;
using ForestRoyale.Gameplay.Systems;
using System;
using UnityEngine;
using VContainer;


namespace ForestRoyale.Gameplay.Combat
{
	public class GameState : MonoBehaviour, IGUIDrawer
	{
		[SerializeField] private DeckDataSO _playerDeck;
		[SerializeField] private DeckDataSO _botDeck;

		[Inject]
		private Arena _arena;

		[Inject]
		private ApplicationEvents _appEvents;

		public enum State
		{
			Meta,
			BattleIntro,
			Battle,
			BattlePaused,
		}

		private State _state;

		private Battle _battle;

		public bool HasActiveBattle => _state == State.Battle;

		private void Start()
		{
			InitializeBattle();
		}

		private void InitializeBattle()
		{
			_state = State.BattleIntro;

			_battle = new Battle();
			_battle.Player.Deck.Initialize(_playerDeck.Cards);
			_battle.Bot.Deck.Initialize(_botDeck.Cards);

			_battle.ResetBattle();
			_appEvents.TriggerBattleCreated(_battle);
		}

		private void StartBattle()
		{
			_state = State.Battle;

			_battle.StartBattle();
			_appEvents.TriggerBattleStarted(_battle);
		}

		public void PauseBattle()
		{
			_state = State.BattlePaused;
			_battle.PauseBattle();
			_appEvents.TriggerBattleEnded(_battle);
		}

		public void ResetBattle()
		{
			_battle.ResetBattle();
			_appEvents.TriggerBattleCreated(_battle);
		}

		public void Update()
		{
			if (_state == State.Battle)
			{
				_battle.Update();
			}
		}

		public void DrawGUI()
		{
			GUILayoutUtils.Label("Battle");
			GUILayoutUtils.Label($"Timer: {_battle.Timer}");

			GUILayout.BeginHorizontal();


			GUI.enabled = !HasActiveBattle;
			if (GUILayout.Button("Start"))
			{
				StartBattle();
			}

			GUI.enabled = _battle?.IsBattleActive ?? false;
			if (GUILayout.Button("Pause"))
			{
				PauseBattle();
			}

			GUI.enabled = _battle != null;
			if (GUILayout.Button("Reset"))
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