using ForestLib.ExtensionMethods;
using ForestRoyale.Gameplay.Cards;
using ForestRoyale.Gameplay.Units;
using ForestRoyale.Gameplay.Units.MonoBehaviours;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VContainer;
using Plane = UnityEngine.Plane;
using Vector3 = UnityEngine.Vector3;

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
		
		[Inject]
		private Arena _arena;

		private List<UnitRoot> _chars = new List<UnitRoot>();

		public CastingState State => _castingState;
		public TroopCardData CardData => _cardData;

		// Gizmos
		private Vector3 _projectedTouchPosition;
		private Vector3 _walkablePosition;
		private Vector3 _targetTilePosition;

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
			// TODO: implement states
			SetState(CastingState.Deploying);
			foreach (UnitRoot character in _chars)
			{
				character.transform.SetParent(charactersRoot);
			}

			// TODO: implement states
			SetState(CastingState.Deployed);
			foreach (UnitRoot character in _chars)
			{
				character.SetState(UnitState.Idle);
			}
		}

		protected void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(_targetTilePosition, 0.3f);
			Gizmos.DrawLine(_walkablePosition, _targetTilePosition);
			
			Gizmos.color = Color.red;
			Gizmos.DrawLine(_projectedTouchPosition, _walkablePosition);
			Gizmos.DrawSphere(_walkablePosition, 0.3f);

			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(_projectedTouchPosition, 0.2f);
		}

		/// <summary>
		/// Get the closest tile to the mouse, that is valid for the troop to be deployed on.
		/// 
		/// A valid tile is a tile that is walkable according to the NavMesh.
		/// </summary>
		protected Vector3 GetClosestTilePosition()
		{
			Ray rayFromTouch = Camera.main.ScreenPointToRay(Input.mousePosition);
			Plane terrainPlain = new Plane(transform.up, 0);
			if (!terrainPlain.Raycast(rayFromTouch, out float distance))
			{
				Debug.LogError("Character preview - can't find tile under touch");
				return Vector3.zero;
			}

			_projectedTouchPosition = rayFromTouch.GetPoint(distance);
			_projectedTouchPosition = _projectedTouchPosition + transform.up * 1f;

			if (!GetClosestWalkablePosition(_projectedTouchPosition, out _walkablePosition))
			{
				return Vector3.zero;
			}
			
			_targetTilePosition = _arena.Grid.WorldToTileCenterPosition(_walkablePosition);

			return _targetTilePosition;
		}

		public bool GetClosestWalkablePosition(Vector3 worldPosition, out Vector3 position)
		{
			// SamplePosition finds the nearest point on NavMesh within maxDistance
			// If a point is found, hit.hit will be true and hit.position will contain the nearest valid position
			//bool hasWalkablePosition = NavMesh.SamplePosition(worldPosition, out hit, _maxDistance, NavMesh.GetAreaFromName("Walkable"));
			bool hasWalkablePosition = NavMesh.SamplePosition(worldPosition, out NavMeshHit navHit, _maxDistance, NavMesh.AllAreas);

			position = navHit.position;
			return hasWalkablePosition;
		}

		public void SetActive(bool value)
		{
			gameObject.SetActive(value);
		}
	}
}
