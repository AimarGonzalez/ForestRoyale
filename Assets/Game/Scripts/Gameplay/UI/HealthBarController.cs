using ForestRoyale.Game.Scripts.Gameplay.Units.MonoBehaviours;
using ForestRoyale.Gameplay.Units;
using UnityEngine;
using UnityEngine.UI;

namespace ForestRoyale.Gameplay.UI
{
	public class HealthBarController : UnitComponent
	{
		[Header("UI References")]
		[Tooltip("The UI Image component that represents the health bar fill")]
		[SerializeField] private Image _healthBarFill;

		[Tooltip("The UI Image component that represents the health bar background")]
		[SerializeField] private Image _background;

		private float _lastHealth;

		private void Start()
		{
			SetupHealthBar();
		}
		
		protected override void OnUnitChanged()
		{
			base.OnUnitChanged();
			
			SetupHealthBar();
		}

		private void SetupHealthBar()
		{
			if (Unit == null)
			{
				return;
			}

			_lastHealth = Unit.CurrentHealth;
			UpdateColor();
			UpdateFillRatio();
		}

		private void Update()
		{
			if (Unit == null)
			{
				UpdateHealthBar(1.0f);
			}
			
			if (Unit.CurrentHealth < _lastHealth)
			{
				UpdateFillRatio();
				PlayHighlightEffect();
				
				_lastHealth = Unit.CurrentHealth;
			}
		}

		private void UpdateColor()
		{
			UISettings.HealthBarColors healthBarColors = null;
			if (Unit.Team == ArenaTeam.Forest)
			{
				healthBarColors = UISettings.instance.EnemyHealthBarColors;
			}
			else
			{
				healthBarColors = UISettings.instance.AllyHealthBarColors;
			}
			
			_background.color = healthBarColors.BackgroundColor;
			_healthBarFill.color = healthBarColors.BarColor;
		}


		private void UpdateFillRatio()
		{
			float healthRatio = Unit.CurrentHealth / Unit.MaxHealth;

			_healthBarFill.fillAmount = healthRatio;
		}
		
		private void UpdateHealthBar(float ratio)
		{
			_healthBarFill.fillAmount = ratio;
		}

		private void PlayHighlightEffect()
		{
			// TODO: Implement highlight effect
		}
	}
}
