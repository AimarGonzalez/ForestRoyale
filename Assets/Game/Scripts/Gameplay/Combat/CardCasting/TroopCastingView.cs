using ForestRoyale.Gameplay.Cards;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

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
	public class TroopCastingView : MonoBehaviour, ICastingView
	{
		public enum CastingState
		{
			Preview,
			Deploying,
			Deployed,
		}

		[ShowInInspector, ReadOnly]
		private CastingState _castingState;

		[ShowInInspector, ReadOnly]
		private TroopCardData _cardData;
		
		// maxDistance is how far to check for a walkable position from the given position
		[ShowInInspector]
		private float _maxDistance = 10.0f;

		private GameObject _troop;

		public CastingState State => _castingState;
		public TroopCardData CardData => _cardData;
		public GameObject Troop => _troop;

		public void SetTroop(TroopCardData cardData, GameObject troop)
		{
			_cardData = cardData;
			_troop = troop;

			_troop.transform.SetParent(this.transform, false);

			SetState(CastingState.Preview);
		}

		private void SetState(CastingState newCastingState)
		{
			if (_castingState == newCastingState)
			{
				return;
			}

			// On Exit State
			switch (_castingState)
			{
				case CastingState.Preview:
					break;
				case CastingState.Deploying:
					break;
				case CastingState.Deployed:
					break;
			}

			_castingState = newCastingState;

			// On Enter State
			switch (newCastingState)
			{
				case CastingState.Preview:
					break;

				case CastingState.Deploying:
					// play deploy animation
					// play clock animation
					break;

				case CastingState.Deployed:
					// hide clock
					// reparent troop to the battlefield
					break;
			}
		}

		protected void Update()
		{
			if (_castingState == CastingState.Preview)
			{
			}

			if (_castingState == CastingState.Deploying)
			{
				// TODO: Implement
			}
		}

		protected void OnDrawGizmos()
		{
			Vector3 mousePosition = Input.mousePosition;
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(worldPosition, 1f);

			Vector3 position;
			if (GetClosestWalkablePosition(worldPosition, out position))
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(position, 1f);
				Gizmos.DrawLine(worldPosition, position);
			}
		}

		/// <summary>
		/// Get the closest tile to the mouse, that is valid for the troop to be deployed on.
		/// 
		/// A valid tile is a tile that is walkable according to the NavMesh.
		/// </summary>
		protected Vector3 GetClosestTilePosition()
		{
			Vector3 mousePosition = Input.mousePosition;
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

			Vector3 position;
			if (GetClosestWalkablePosition(worldPosition, out position))
			{
				return position;
			}

			return worldPosition;
		}

		public bool GetClosestWalkablePosition(Vector3 worldPosition, out Vector3 position)
		{
			NavMeshHit hit;

			// SamplePosition finds the nearest point on NavMesh within maxDistance
			// If a point is found, hit.hit will be true and hit.position will contain the nearest valid position
			bool hasWalkablePosition = NavMesh.SamplePosition(worldPosition, out hit, _maxDistance, NavMesh.GetAreaFromName("Walkable"));

			position = hit.position;
			return hasWalkablePosition;
		}

		public void SetActive(bool value)
		{
			gameObject.SetActive(value);
		}
	}
}
