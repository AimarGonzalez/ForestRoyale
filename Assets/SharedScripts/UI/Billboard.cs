using UnityEngine;

// This script is used to make a UI element face the camera.
// It can be used in two modes:
// - TrueBillboard: Rotate to face the camera
// - SameAsCameraDirection: Rotate to face the camera direction

[ExecuteInEditMode]
public class Billboard : MonoBehaviour
{
	private Transform _mainCamera;
	private Vector3 _worldDirection;

	public enum BillbordMode
	{
		TrueBillboard, // Rotate to face the camera
		SameAsCameraDirection, // Rotate to face the camera direction
	}

	[SerializeField] private BillbordMode mode;

	private void Start()
	{
		_mainCamera = Camera.main.transform;
	}

	private void OnValidate()
	{
	}

	private void Update()
	{
		if (mode == BillbordMode.TrueBillboard)
		{
			transform.LookAt(_mainCamera);
		}
		else
		{
			// 180 degrees from camera direction
			transform.rotation = _mainCamera.rotation * Quaternion.AngleAxis(180, Vector3.up);
		}
	}
}

