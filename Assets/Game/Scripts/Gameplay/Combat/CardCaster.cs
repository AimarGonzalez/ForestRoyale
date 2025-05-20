using ForestRoyale.Gameplay.Systems;
using UnityEngine;
using VContainer;

namespace ForestRoyale.Gameplay.Combat
{
	public class CardCaster : MonoBehaviour
	{
		[SerializeField, Range(0f, 1f)]
		private float castingLinePosition = 0.3f;

		[Inject]
		private ApplicationEvents _applicationEvents;

		private Hand _hand;
		private Deck _deck;
		private Player _player;

		private void Awake()
		{
			Debug.Log($"CardCaster - Awake ({_applicationEvents})");
		}

		private void Start()
		{
			Subscribe();
		}

		private void Subscribe()
		{
			_applicationEvents.OnBattleStarted += OnBattleStarted;
		}

		private void Unsubscribe()
		{
			_applicationEvents.OnBattleStarted -= OnBattleStarted;
		}

		private void OnDestroy()
		{
			Unsubscribe();
		}

		private void OnBattleStarted(Battle battle)
		{
			Debug.Log($"CardCaster - OnBattleStarted");
			_player = battle.Player;
			_hand = battle.Player.Hand;
			_deck = battle.Player.Deck;
			
		}

		// ------------------------------------------------

		private void OnDrawGizmos()
		{
			Camera camera = Camera.main;
			if (camera == null)
			{
				return;
			}

			float screenHeight = camera.pixelHeight;
			float screenWidth = camera.pixelWidth;

			Vector3 leftPoint = camera.ScreenToWorldPoint(new Vector3(0, screenHeight * castingLinePosition, 10));
			Vector3 rightPoint = camera.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight * castingLinePosition, 10));

			Gizmos.color = Color.red;
			Gizmos.DrawLine(leftPoint, rightPoint);
		}
	}
}
