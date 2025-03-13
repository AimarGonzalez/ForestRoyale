using UnityEngine;
using System;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ForestRoyale
{
    [ExecuteInEditMode]
    public class DamageHighlightController : MonoBehaviour
    {
        private float _currentIntensity = 0f;
        private float _targetIntensity = 0f;
        private bool _isFlashing = false;
        private float _flashTimer = 0f;
        
#if UNITY_EDITOR
        // Use this param to previsualize the effect in EditMode
        [SerializeField, Range(0, 1)]
        private float _previewIntensity = 0f;
#endif
        
        [SerializeField, Range(0, 1)]
        private float _maxIntensity = 0.3f;
       
        [SerializeField, Min(0.001f)]
        private float _flashDuration = 1f;
        
        [NonSerialized]
        private Material _material; 


        void OnEnable()
        {
            FetchMaterial();
#if UNITY_EDITOR
            EditorApplication.update += EditorUpdate;
#endif
        }

        void OnDisable()
        {
#if UNITY_EDITOR
            EditorApplication.update -= EditorUpdate;
#endif
        }

        private void FetchMaterial()
        {
            if(_material != null)
            {
                return;
            }

            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                // Use renderer.sharedMaterial if we're in EditMode. Using renderer.material 
                // would instance a temporal material, which would pollute our scene.
                _material = Application.isPlaying ? renderer.material : renderer.sharedMaterial;
            }
            else
            {
                Debug.LogError("No Renderer found on the GameObject.");
            }
        }


#if UNITY_EDITOR
        void OnValidate()
        { 
            UpdateMaterial(_previewIntensity);
        }

        private void EditorUpdate()
        {
            if (_isFlashing)
            {
                // Issue: Update() not called regularly in EditMode, so we can't see the color animation.
                // Solution: Mark the object as dirty to force a call to Update()
                EditorUtility.SetDirty(this);
            }
        }
#endif

        void Update()
        {
            if (_isFlashing)
            {
                UpdateFlashEffect();
            }
        }
        
        private void UpdateFlashEffect()
        {
            if (_material == null)
                return;
            
            _flashTimer += Time.deltaTime;
            if (_flashTimer < _flashDuration)
            {
                // Smoothly interpolate the current intensity to the target
                float progress = _flashTimer / _flashDuration;
                _currentIntensity = Mathf.Lerp(_maxIntensity, _targetIntensity, progress);
            }
            else
            {
                _isFlashing = false;
                _targetIntensity = 0f;
                _currentIntensity = 0f;
            }
            
            UpdateMaterial(_currentIntensity);
        }
        
        private void UpdateMaterial(float currentIntensity)
        {
            if (_material != null)
            {
                _material.SetFloat("_DamageIntensity", currentIntensity * _maxIntensity);
            }
        }

        public void FlashDamage()
        {
            _isFlashing = true;
            _flashTimer = 0f;
            _targetIntensity = 0f;
            _currentIntensity = _maxIntensity;
            
#if UNITY_EDITOR
            _previewIntensity = 0; // Reset the previsualization value when animating damage
#endif
        }
    }
} 