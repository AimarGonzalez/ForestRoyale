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

		[Flags]
		public enum State
		{
			Meta = 1 << 0,
			BattleIntro = 1 << 1,
			Battle = 1 << 2,
			BattlePaused = 1 << 3,
			Any = Meta | BattleIntro | Battle | BattlePaused
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

			_battle = new Battle();

			// IMPROVE: Refactor to Initialize(BattleSettings)
			_battle.Player.Deck.Initialize(_playerDeck.Cards);
			_battle.Bot.Deck.Initialize(_botDeck.Cards);

			ResetBattle();
		}

		public void StartBattle()
		{
			_battle.StartBattle();

			SetState(State.Battle);
			_appEvents.TriggerBattleStarted(_battle);
		}

		public void PauseBattle()
		{
			_battle.PauseBattle();
			
			SetState(State.BattlePaused);
			_appEvents.TriggerBattlePaused(_battle);
		}

		public void ResetBattle()
		{
			_battle.ResetBattle();

			SetState(State.BattleIntro);
			_appEvents.TriggerBattleCreated(_battle);
		}

		public void Update()
		{
			if (_state == State.Battle)
			{
				_battle.Update();
			}
		}

		private void SetState(State newState)
		{
			if (_state == newState)
			{
				return;
			}

			State oldState = _state;
			_state = newState;
			_appEvents.TriggerGameStateChanged(oldState, newState);

			if (oldState == State.BattleIntro)
			{
				_battle.PopulateInitialHand();
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

			GUI.enabled = HasActiveBattle;
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