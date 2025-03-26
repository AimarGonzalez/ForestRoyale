using UnityEngine;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ForestRoyale.VFX
{
	[ExecuteInEditMode]
	public class DamageHighlightController : MonoBehaviour
	{
		[SerializeField, Range(0, 1)]
		private float _maxIntensity = 0.3f;

		[SerializeField, Min(0.001f)]
		private float _flashDuration = 1f;

		[SerializeField]
		private Ease _flashEase = Ease.Linear;

		[SerializeField]
		private Color _damageColor = Color.red;

#if UNITY_EDITOR
		// Use this param to previsualize the effect in EditMode
		[SerializeField, Range(0, 1)]
		private float _previewIntensity = 0f;

#endif

		private Material _material;
		private Tweener _flashTween;

		void OnEnable()
		{
			FetchMaterial();
#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				EditorApplication.update += EditorUpdate;
			}
#endif
		}

		void OnDisable()
		{
			_flashTween?.Kill();
#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				EditorApplication.update -= EditorUpdate;
			}
#endif
		}

		private void FetchMaterial()
		{
			if (_material != null)
			{
				return;
			}

			Renderer renderer = GetComponent<Renderer>();
			if (renderer != null)
			{
				// Use renderer.sharedMaterial if we're in EditMode. Using renderer.material 
				// would instance a temporal material, which would pollute our scene.
				_material = Application.isPlaying ? renderer.material : renderer.sharedMaterial;

				// Set initial color after getting material
				UpdateMaterial(0f, _damageColor);
			}
			else
			{
				Debug.LogError("No Renderer found on the GameObject.");
			}
		}

#if UNITY_EDITOR
		void OnValidate()
		{
			UpdateMaterial(_previewIntensity, _damageColor);
		}
#endif

		private void UpdateMaterial(float currentIntensity, Color color)
		{
			if (_material != null)
			{
				_material.SetFloat("_DamageIntensity", currentIntensity * _maxIntensity);
				_material.SetColor("_DamageColor", color);
			}
		}

		public void FlashDamage()
		{
			if (_material == null)
			{
				return;
			}

			_flashTween?.Kill();
			_flashTween = DOTween.To(
					() => 1f,
					(currentValue) => { UpdateMaterial(currentValue, _damageColor); },
					0f, _flashDuration)
				.SetEase(_flashEase);


#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				_flashTween.SetUpdate(UpdateType.Manual, true);
			}

			// Reset the previsualization value when animating damage
			_previewIntensity = 0;
#endif
		}

#if UNITY_EDITOR
		private void EditorUpdate()
		{
			_flashTween?.ManualUpdate(Time.deltaTime, Time.unscaledDeltaTime);
		}
#endif
	}
}