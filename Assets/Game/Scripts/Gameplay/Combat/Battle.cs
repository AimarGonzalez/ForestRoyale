using UnityEngine;

namespace ForestRoyale.Gameplay.Combat
{
	public class Battle : MonoBehaviour
	{
		[SerializeField] private Player _player;
		[SerializeField] private Player _bot;
		[SerializeField] private float _battleDuration = 180f; // 3 minutes

		private float _currentTime;
		private bool _isBattleActive;

		public Player Player => _player;
		public Player Bot => _bot;
		public float RemainingTime => Mathf.Max(0, _battleDuration - _currentTime);
		public bool IsBattleActive => _isBattleActive;

		private void Start()
		{
			InitializeBattle();
		}

		private void Update()
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

		private void InitializeBattle()
		{
			_currentTime = 0f;
			_isBattleActive = true;

			// Initialize player and bot hands
			_player.PopulateInitialHand();
			_bot.PopulateInitialHand();
		}

		private void EndBattle()
		{
			_isBattleActive = false;
			// TODO: Implement battle end logic (determine winner, show results, etc.)
		}

		public void PauseBattle()
		{
			_isBattleActive = false;
		}

		public void ResumeBattle()
		{
			_isBattleActive = true;
		}
	}
}