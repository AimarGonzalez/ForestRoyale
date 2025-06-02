using ForestLib.ExtensionMethods;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Cards.CardStats;
using ForestRoyale.Gameplay.Units;
using ForestRoyale.Gameplay.Units.MonoBehaviours;
using Sirenix.OdinInspector;
using System.Collections.Generic;
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

		[SerializeField]
		private Transform _castingMarker;

		[ShowInInspector, ReadOnly]
		private CastingState _castingState;

		[ShowInInspector, ReadOnly]
		private TroopCardData _cardData;

		// maxDistance is how far to check for a walkable position from the given position
		[ShowInInspector]
		private float _maxDistance = 10.0f;

		private Transform squadTransform;
		
		private List<UnitRoot> _chars = new List<UnitRoot>();

		public CastingState State => _castingState;
		public TroopCardData CardData => _cardData;

		public void SetTroop(TroopCardData cardData, Transform troop, ArenaTeam team, UnitState state)
		{
			_cardData = cardData;

			SetState(CastingState.Preview);

			SetParentAndCacheTroops(troop);
			SetStartingProperties(team, state);
		}

		private void SetParentAndCacheTroops(Transform squadTransform)
		{
			_chars.Clear();

			if (squadTransform.TryGetComponent(out UnitRoot singleCharacter))
			{
				// Single unit
				_chars.Add(singleCharacter);
				singleCharacter.transform.SetParent(transform, false);
			}
			else
			{
				// Multiple units
				foreach (UnitRoot squadCharacter in squadTransform)
				{
					_chars.Add(squadCharacter);
					squadCharacter.transform.SetParent(transform, false);
				}
				
				// Get rid of the squad root object
				Destroy(squadTransform.gameObject);
			}
		}

		private void SetStartingProperties(ArenaTeam team, UnitState state)
		{
			foreach (UnitRoot character in _chars)
			{
				character.StartingTeam = team;
				character.StartingState = state;
			}
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
				Vector3 tilePosition = GetClosestTilePosition();
				transform.position = tilePosition;
			}

			if (_castingState == CastingState.Deploying)
			{
				// TODO: Implement
			}
		}

		public void Cast(Transform charactersRoot)
		{
			foreach (UnitRoot character in _chars)
			{
				character.transform.SetParent(charactersRoot);
			}

			// TODO: implement states
			SetState(CastingState.Deploying);
			SetState(CastingState.Deployed);
		}

		protected void OnDrawGizmos()
		{
			Vector3 mousePosition = Input.mousePosition;
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x,mousePosition.y, 20f));

			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(worldPosition, 0.5f);

			Vector3 position;
			if (GetClosestWalkablePosition(worldPosition, out position))
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(position, 2f);
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
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x,mousePosition.y, 20f));

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
			//bool hasWalkablePosition = NavMesh.SamplePosition(worldPosition, out hit, _maxDistance, NavMesh.GetAreaFromName("Walkable"));
			bool hasWalkablePosition = NavMesh.SamplePosition(worldPosition, out hit, _maxDistance, NavMesh.AllAreas);

			position = hit.position;
			return hasWalkablePosition;
		}

		public void SetActive(bool value)
		{
			gameObject.SetActive(value);
		}
	}
}
