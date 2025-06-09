using ForestRoyale.Core.UI;
using ForestRoyale.Gameplay.Units;
using ForestRoyale.Gameplay.Units.MonoBehaviours;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ForestRoyale.Gameplay.UI
{
	public class HealthBarController : UnitComponent, IUnitChangeListener
	{
		[Header("UI References")]
		[Tooltip("The UI Image component that represents the health bar fill")]
		[SerializeField] private Image _healthBarFill;

		[Tooltip("The UI Image component that represents the health bar frame")]
		[SerializeField] private Image _healthBarFrame;

		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[SerializeField] private ArenaTeam _team = ArenaTeam.Player;

		[BoxGroup(DebugUI.Group), PropertyOrder(DebugUI.Order)]
		[Range(0f, 1f)]
		[SerializeField] private float _healthRatio = 1f;

		private UISettings UISettings => GameSettings.Instance.UISettings;

		private float _lastHealth;

		private void Start()
		{
			SetupHealthBar();
		}

		void IUnitChangeListener.OnUnitChanged(Unit oldUnit, Unit newUnit)
		{
			SetupHealthBar();
		}

		private void SetupHealthBar()
		{
			UpdateColor();
			UpdateFillRatio();
		}

		void OnValidate()
		{
			// PATCH: Obort. GameSettings.Instance can't be obtained if the scene is not loaded.
			// TODO: I need a reliable way to load the settings at any moment.
			if (!gameObject.scene.isLoaded)
			{
				return;
			}
			
			SetColor(_team);
			SetFillRatio(_healthRatio);
		}

		private void Update()
		{
			if (Unit == null)
			{
				return;
			}

			if (Unit.CurrentHealth != _lastHealth)
			{
				UpdateFillRatio();
				PlayHighlightEffect();
			}
		}

		private void UpdateColor()
		{
			if(Unit == null)
			{
				return;
			}

			SetColor(Unit.Team);
		}

		private void SetColor(ArenaTeam team)
		{
			UISettings.HealthBarColors healthBarColors;
			if (team == ArenaTeam.Forest)
			{
				healthBarColors = UISettings.EnemyHealthBarColors;
			}
			else
			{
				healthBarColors = UISettings.AllyHealthBarColors;
			}

			_healthBarFrame.color = healthBarColors.FrameColor;
			_healthBarFill.color = healthBarColors.BarColor;
		}


		private void UpdateFillRatio()
		{
			if(Unit == null)
			{
				return;
			}
			
			SetFillRatio(Unit.CurrentHealth / Unit.MaxHealth);

			_lastHealth = Unit.CurrentHealth;
		}
		
		private void SetFillRatio(float healthRatio)
		{
			_healthBarFill.fillAmount = healthRatio;
		}


		private void PlayHighlightEffect()
		{
			// TODO: Implement highlight effect
		}
	}
}
