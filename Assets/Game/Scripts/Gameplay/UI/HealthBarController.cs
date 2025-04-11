using ForestRoyale.Game.Scripts.Gameplay.Units.MonoBehaviours;
using ForestRoyale.Gameplay.Units;
using ForestRoyale.Gui;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ForestRoyale.Gameplay.UI
{
	public class HealthBarController : UnitComponent
	{
		[Header("UI References")]
		[Tooltip("The UI Image component that represents the health bar fill")]
		[SerializeField] private Image _healthBarFill;

		[Tooltip("The UI Image component that represents the health bar frame")]
		[SerializeField] private Image _healthBarFrame;

		[BoxGroup(InspectorConstants.DebugGroup), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		[SerializeField] private ArenaTeam _team = ArenaTeam.Player;

		[BoxGroup(InspectorConstants.DebugGroup), PropertyOrder(InspectorConstants.DebugGroupOrder)]
		[Range(0f, 1f)]
		[SerializeField] private float _healthRatio = 1f;

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
			UpdateColor();
			UpdateFillRatio();
		}

		void OnValidate()
		{
			SetColor(_team);
			SetFillRatio(_healthRatio);
		}

		private void Update()
		{
			if (Unit == null)
			{
				return;
			}

			if (Unit.CurrentHealth < _lastHealth)
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
				healthBarColors = UISettings.instance.EnemyHealthBarColors;
			}
			else
			{
				healthBarColors = UISettings.instance.AllyHealthBarColors;
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
