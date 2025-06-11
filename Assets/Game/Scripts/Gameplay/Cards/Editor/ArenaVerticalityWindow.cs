using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace ForestRoyale.Gameplay.Editor
{
	public class ArenaVerticalityWindow : OdinEditorWindow
	{
		[MenuItem("ForestRoyale/Arena Verticality")]
		private static void OpenWindow()
		{
			GetWindow<ArenaVerticalityWindow>("Arena Verticality").Show();
		}

		private GameObject _arena;

		[SerializeField, HideInInspector]
		private bool _isVertical;

		private bool IsArenaNull => _arena == null;

		protected override void OnEnable()
		{
			base.OnEnable();
			FindAndInitializeArena();
		}


		[InfoBox("Arena not found in scene!", InfoMessageType.Error, nameof(IsArenaNull))]
		[Button("Refresh Arena Reference")]
		private void FindAndInitializeArena()
		{
			_arena = GameObject.Find("Arena");
			if (_arena != null)
			{
				_isVertical = Mathf.Approximately(_arena.transform.rotation.eulerAngles.x, 270f);
			}
		}

		private string RotateArenaButtonLabel => $"Rotate Arena: {(_isVertical ? "Horizontal" : "Vertical")}";

		[Button("@"+nameof(RotateArenaButtonLabel)), GUIColor(0.3f, 0.8f, 0.3f), EnableIf("@_arena != null"), PropertyOrder(-1)]
		private void UpdateArenaRotation()
		{
			if (_arena == null)
			{
				return;
			}

			Undo.SetCurrentGroupName(RotateArenaButtonLabel);

			_isVertical = !_isVertical;

			Undo.RecordObject(_arena.transform, "Toggle Arena Rotation");
			_arena.transform.rotation = Quaternion.Euler(_isVertical ? 270f : 0f, 0f, 0f);
			
			BoxCollider2D[] boxColliders = FindObjectsByType<BoxCollider2D>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (BoxCollider2D collider in boxColliders)
			{
				Undo.RecordObject(collider.transform, "Toggle Collider Rotation");
				Vector3 currentRotation = collider.transform.rotation.eulerAngles;
				collider.transform.rotation = Quaternion.Euler(0f, currentRotation.y, currentRotation.z);
			}

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
		}
	}
}