using UnityEngine;

namespace ForestRoyale
{
    public class DamageHighlightController : MonoBehaviour
    {
        private Material _material; 
        private float _currentIntensity = 0f;
        private float _targetIntensity = 0f;

        [SerializeField]
        private float _fadeSpeed = 2f;
        private bool _isFlashing = false;
        private float _flashDuration = 0.5f;
        private float _flashTimer = 0f;

        void Start()
        {
            // Get the material from the renderer
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                _material = renderer.material;
            }
            else
            {
                Debug.LogError("No Renderer found on the GameObject.");
            }
        }

        void Update()
        {
            if (_material == null) return;

            if (_isFlashing)
            {
                _flashTimer += Time.deltaTime;
                if (_flashTimer >= _flashDuration)
                {
                    _isFlashing = false;
                    _targetIntensity = 0f;
                }
            }

            // Smoothly interpolate the current intensity to the target
            _currentIntensity = Mathf.Lerp(_currentIntensity, _targetIntensity, Time.deltaTime * _fadeSpeed);
            _material.SetFloat("_DamageIntensity", _currentIntensity);
        }

        public void FlashDamage()
        {
            _isFlashing = true;
            _flashTimer = 0f;
            _targetIntensity = 1f;
        }
    }
} 