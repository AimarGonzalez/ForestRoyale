using ForestRoyale.Core.UI;
using System;
using UnityEngine;
using VContainer;


namespace ForestRoyale.Gameplay.Combat
{
	// IMPROVE: Split between BattleTools and Battle
	public class Battle
	{
		private float _battleDuration = 180f;
		private float _currentTime;
		private bool _isBattleActive;

		private Player _player;
		private Player _bot;

		public Player Player => _player;
		public Player Bot => _bot;
		public float RemainingTime => Mathf.Max(0, _battleDuration - _currentTime);
		public bool IsBattleActive => _isBattleActive;

		public Battle(float duration)
		{
			_battleDuration = duration;
			_player = new Player();
			_bot = new Player();
		}

		public void Update()
		{
			if (!_isBattleActive)
			{
				return;
			}

			_currentTime += Time.deltaTime;
			if (_currentTime >= _battleDuration)
			{
				EndBattle();
			}
		}

		public void ResetBattle()
		{
			_currentTime = 0f;
			_isBattleActive = false;
			
			_player.Hand.Clear();
			_bot.Hand.Clear();
		}

		public void StartBattle()
		{
			_isBattleActive = true;
			
			// Initialize player and bot hands
			_player.PopulateInitialHand();
			_bot.PopulateInitialHand();
		}

		public void EndBattle()
		{
			_isBattleActive = false;
			// TODO: Implement battle end logic (determine winner, show results, etc.)
		}
	}
}