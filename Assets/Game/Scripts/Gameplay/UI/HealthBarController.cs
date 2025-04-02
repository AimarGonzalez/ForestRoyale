using ForestRoyale.Gameplay.Units;
using UnityEngine;
using UnityEngine.UI;

namespace ForestRoyale.Gameplay.UI
{
	public class HealthBarController : MonoBehaviour
	{
		[Header("UI References")]
		[Tooltip("The UI Image component that represents the health bar fill")]
		[SerializeField] private Image healthBarFill;

		private IDamageable _unit;
		private float _lastHealth;
		private void Awake()
		{
			_unit = GetComponentInParent<IDamageable>();
			if (_unit == null)
			{
				Debug.LogError("HealthBarController: No IDamageable component found in parent hierarchy!");
				return;
			}
		}

		private void Start()
		{
			UpdateHealthBar();
		}

		private void Update()
		{
			if (_unit.Health < _lastHealth)
			{
				UpdateHealthBar();
				PlayHighlightEffect();
			}
		}

		private void UpdateHealthBar()
		{
			float healthRatio = _unit.Health / _unit.MaxHealth;

			healthBarFill.fillAmount = healthRatio;

			_lastHealth = _unit.Health;
		}

		private void PlayHighlightEffect()
		{
			// TODO: Implement highlight effect
		}
	}
}
