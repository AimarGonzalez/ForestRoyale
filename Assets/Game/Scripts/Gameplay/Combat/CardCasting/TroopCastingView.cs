using ForestRoyale.Gameplay.Cards;
using UnityEngine;

namespace ForestRoyale.Gameplay.Combat
{
	/// <summary>
	/// This component lives in the TroopCastingView prefab.
	/// It is used to:
	///  - display the troop preview
	///  - display missing elixir cost
	///  - display the deploying clock
	///  - provide the troop instance
	/// </summary>
	public class TroopCastingView : MonoBehaviour
	{
		public enum State
		{
			Hidden,
			Preview,
			Deploying,
			Deployed,
		}

		[ShowInInspector, ReadOnly]
		private State _state;

		[ShowInInspector, ReadOnly]
		private TroopCardData _cardData;

		private GameObject _troop;

		public State State => _state;
		public TroopCardData CardData => _cardData;
		public GameObject Troop => _troop;

		public void SetTroop(TroopCardData cardData, GameObject troop)
		{
			_cardData = cardData;
			_troop = troop;

			_troop.transform.SetParent(this.transform, false);

			SetState(State.Hidden);
		}

		private void SetState(State newState)
		{
			if (_state == newState)
			{
				return;
			}

			// On Exit State
			switch (_state)
			{
				case State.Hidden:
					break;
				case State.Preview:
					break;
				case State.Deploying:
					break;
				case State.Deployed:
					break;
			}

			_state = newState;

			// On Enter State
			switch (newState)
			{
				case State.Hidden:
					_troop.SetActive(false);
					break;

				case State.Preview:
					_troop.SetActive(true);
					break;

				case State.Deploying:
					_troop.SetActive(true);

					// play deploy animation
					// play clock animation
					break;

				case State.Deployed:
					// hide clock
					// reparent troop to the battlefield
					break;
			}
		}

		protected void Update()
		{
			if (_state == State.Deploying)
			{
				UpdateDeploying();
			}
		}

		/// <summary>
		/// Get the closest tile to the mouse, that is valid for the troop to be deployed on.
		/// 
		/// A valid tile is a tile that is walkable according to the NavMesh.
		/// </summary>
		protected Vector3 GetClosestTilePosition()
		{




		}
	}
}
