using ForestRoyale.Core;
using ForestRoyale.Core.UI;
using ForestRoyale.Gameplay.Units;
using ForestRoyale.Gameplay.Units.MonoBehaviours;
using Game.Scripts.Gameplay.Cards.CardStats;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ForestRoyale.Gameplay.Combat
{
	public class Battle : MonoBehaviour, IGUIDrawer
	{
		[SerializeField] private Player _player;
		[SerializeField] private Player _bot;
		[SerializeField] private float _battleDuration = 180f; // 3 minutes

		[SerializeField] private GameObject _arena;

		private float _currentTime;
		private bool _isBattleActive;

		public Player Player => _player;
		public Player Bot => _bot;
		public float RemainingTime => Mathf.Max(0, _battleDuration - _currentTime);
		public bool IsBattleActive => _isBattleActive;

		private void Start()
		{
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

		private void ResetTowers()
		{
			List<Unit> towers = _arena.GetComponentsInChildren<UnitRoot>()
										.Where(root => root.Unit.UnitType == UnitType.ArenaTower)
										.Select(root => root.Unit).ToList();

			foreach (var tower in towers)
			{
				tower.Reset();
			}
		}

		public void DrawGUI()
		{
			GUILayout.Label("Battle");
			GUILayout.Label($"Remaining Time: {RemainingTime}");

			GUILayout.BeginHorizontal();


			if (GUILayout.Button("Start Battle"))
			{
				InitializeBattle();
			}

			if (GUILayout.Button("End Battle"))
			{
				EndBattle();
			}

			GUILayout.EndHorizontal();

			if (GUILayout.Button("Respawn towers"))
			{
				ResetTowers();
			}
		}
	}
}