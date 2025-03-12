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

        [SerializeField, Range(0, 1)]
        private float _maxIntensity = 0.3f;
       
#if UNITY_EDITOR
        // Use this param to previsualize the effect in EditMode
        [SerializeField, Range(0, 1)]
        private float _previewIntensity = 0f;
#endif

        [SerializeField, Min(0.001f)]
        private float _flashDuration = 1f;
        
        [NonSerialized]
        private Material _material; 


        void Start()
        {
            // Get the material from the renderer
            InitializeMaterial();
        }

        private void InitializeMaterial()
        {
            if(_material != null)
            {
                return;
            }

            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                _material = Application.isPlaying ? renderer.material : renderer.sharedMaterial;
            }
            else
            {
                Debug.LogError("No Renderer found on the GameObject.");
            }
        }

        void Update()
        {
            if (_material == null || !Application.isPlaying)
                return;

            UpdateDamageEffect(Time.deltaTime, ref _currentIntensity);
        }

        // This method contains the logic from the original Update method
        private void UpdateDamageEffect(float deltaTime, ref float currentIntensity)
        {
            if (_isFlashing)
            {
                _flashTimer += deltaTime;
                if (_flashTimer >= _flashDuration)
                {
                    _isFlashing = false;
                    _targetIntensity = 0f;
                }
           
                // Smoothly interpolate the current intensity to the target
                float progress = _flashTimer / _flashDuration;
                currentIntensity = Mathf.Lerp(_maxIntensity, _targetIntensity, progress);
              
                UpdateMaterial(currentIntensity);
            }
        }

         private void UpdateMaterial(float currentIntensity)
        {
            if (_material != null)
            {
                _material.SetFloat("_DamageIntensity", currentIntensity);
            }
        }

        public void FlashDamage()
        {
            _isFlashing = true;
            _flashTimer = 0f;
            _targetIntensity = 0f;
            _currentIntensity = _maxIntensity;
                
#if UNITY_EDITOR
            _previewIntensity = _maxIntensity;
#endif            
        }

#if UNITY_EDITOR

        void OnValidate()
        { 
             UpdateMaterial(_previewIntensity * _maxIntensity);
        }

        void OnEnable()
        {
            InitializeMaterial();
            if (!Application.isPlaying)
            {
                EditorApplication.update += OnEditorUpdate;           
            }
        }

        void OnDisable()
        {
            if (!Application.isPlaying)
            {
                EditorApplication.update -= OnEditorUpdate;
            }
        }

        void OnEditorUpdate()
        {
            if (_material == null || Application.isPlaying)
                return;
            
            UpdateDamageEffect(Time.deltaTime, ref _previewIntensity);
        }
#endif
    }
} 