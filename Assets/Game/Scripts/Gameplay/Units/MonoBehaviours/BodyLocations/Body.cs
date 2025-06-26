using ForestLib.ExtensionMethods;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ForestRoyale.Gameplay.Units.MonoBehaviours.BodyLocations
{
	public class Body : UnitComponent, IUnitChangeListener
	{
		[SerializeField] private MeshRenderer _bodyMeshRenderer;
		private Transform _projectileSourceLocation;

		[ShowInInspector]
		private Vector3 _center;

		[ShowInInspector]
		private float _radius;

		public Vector3 Center => _bodyMeshRenderer.bounds.center;
		public float Radius => _radius;

		public Vector3 ProjectileSourcePosition => _projectileSourceLocation.position;
		public Vector3 ProjectileTargetPosition => Center;

		protected override void Awake()
		{
			base.Awake();

			if (this.TryGetComponentInChildren(out ProjectileSourceLocation projectileSourcePosition, includeInactive: true))
			{
				_projectileSourceLocation = projectileSourcePosition.transform;
			}

			if (_bodyMeshRenderer)
			{
				Bounds bounds = _bodyMeshRenderer.localBounds;
				_radius = bounds.size.x / 2;
			}
		}

		public void OnUnitChanged(Unit oldUnit, Unit newUnit)
		{
			ValidateBody();
		}

		private void ValidateBody()
		{
			if (Unit == null)
			{
				return;
			}

			Debug.Assert(_bodyMeshRenderer, $"['{Unit.Id}'] Body: Mesh not set");

			if (Unit.CombatStats.IsRanged)
			{
				Debug.Assert(_projectileSourceLocation != null, $"['{Unit.Id}'] Body: Ranged character is missing the {nameof(ProjectileSourceLocation)} component");
			}
		}

		public Vector3 GetTargetPositionFrom(Vector3 attackerPosition)
		{
			Vector3 bodyPosition = Center;
			Bounds bounds = _bodyMeshRenderer.bounds;

			// Calculate direction from attacker to body center
			Vector3 direction = (bodyPosition - attackerPosition).normalized;

			// Create a ray from attacker position towards body center
			Ray ray = new Ray(attackerPosition, direction);

			// Check if ray intersects with bounding box
			if (bounds.IntersectRay(ray, out float distance))
			{
				// Return the intersection point
				return attackerPosition + direction * distance;
			}

			// Fallback: if no intersection (shouldn't happen in normal cases), return center
			return bodyPosition;
		}

		private void OnDrawGizmos()
		{
			//Draw schere at Center with radius
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(Center, 0.1f);
		}
	}
}