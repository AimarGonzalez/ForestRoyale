using UnityEngine;
using UnityEngine.UI;
using ForestRoyale.Gameplay.Units;
namespace ForestRoyale.Gameplay.UI
{
	public class HealthBarController : MonoBehaviour
	{
		[Header("UI References")]
		[Tooltip("The UI Image component that represents the health bar fill")]
		[SerializeField] private Image healthBarFill;

		private Unit _unit;
		private float _lastHealth;
		private void Awake()
		{
			_unit = GetComponentInParent<Unit>();
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
			if (_unit.RemainingHealth < _lastHealth)
			{
				UpdateHealthBar();
				PlayHighlightEffect();
			}
		}

		private void UpdateHealthBar()
		{
			float healthRatio = _unit.RemainingHealth / _unit.MaxHealth;

			healthBarFill.fillAmount = healthRatio;

			_lastHealth = _unit.RemainingHealth;
		}

		private void PlayHighlightEffect()
		{
			// TODO: Implement highlight effect
		}
	}
}
