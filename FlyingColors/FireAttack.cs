using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using Csla.Rules;

namespace FlyingColors
{
	[Serializable]
	public class FireAttack : BusinessBase<FireAttack>
	{
		public static readonly PropertyInfo<BattleShip> FiringShipProperty = RegisterProperty<BattleShip>(c => c.FiringShip);
		/// <Summary>
		/// Gets or sets the FiringShip value.
		/// </Summary>
		public BattleShip FiringShip
		{
			get { return GetProperty(FiringShipProperty); }
			set { SetProperty(FiringShipProperty, value); }
		}

		public static readonly PropertyInfo<BattleShip> TargetShipProperty = RegisterProperty<BattleShip>(c => c.TargetShip);
		/// <Summary>
		/// Gets or sets the TargetShip value.
		/// </Summary>
		public BattleShip TargetShip
		{
			get { return GetProperty(TargetShipProperty); }
			set { SetProperty(TargetShipProperty, value); }
		}

		public static readonly PropertyInfo<int> RangeProperty = RegisterProperty<int>(c => c.Range);
		/// <Summary>
		/// Gets or sets the Range value.
		/// </Summary>
		public int Range
		{
			get { return GetProperty(RangeProperty); }
			set { SetProperty(RangeProperty, value); }
		}

		public static readonly PropertyInfo<bool> IsPointBlankProperty = RegisterProperty<bool>(c => c.IsPointBlank);
		/// <Summary>
		/// Gets or sets the IsPointBlank value.
		/// </Summary>
		public bool IsPointBlank
		{
			get { return GetProperty(IsPointBlankProperty); }
			set { SetProperty(IsPointBlankProperty, value); }
		}

		private class RangeRule : BusinessRule
		{
			public RangeRule() : base(RangeProperty)
			{
				AffectedProperties.Add(IsPointBlankProperty);
			}

			protected override void Execute(RuleContext context)
			{
				var fireAttack = (FireAttack)context.Target;
				if (fireAttack.Range < 0)
				{
					context.AddErrorResult("Range must be positive integer.");
				}
				else if (fireAttack.Range == 0)
				{
					context.OutputPropertyValues.Add(IsPointBlankProperty, true);
				}
			}
		}

		private class IsPointBlankRule : BusinessRule
		{
			public IsPointBlankRule()
				: base(IsPointBlankProperty)
			{
				AffectedProperties.Add(SimultaneousDefensiveFireProperty);
			}

			protected override void Execute(RuleContext context)
			{
				var fireAttack = (FireAttack)context.Target;
				if (fireAttack.IsPointBlank && fireAttack.DefensiveFireAttack.IsPointBlank)
				{
					context.AddOutValue(SimultaneousDefensiveFireProperty, true);
				}
				else
				{
					context.AddOutValue(SimultaneousDefensiveFireProperty, false);
				}
			}
		}

		
		#region Business Methods

		// TODO: add public properties and methods

		#endregion

		#region Validation Rules

		protected override void AddBusinessRules()
		{
			base.AddBusinessRules();

			// TODO: add validation rules
		}

		#endregion

		#region Authorization Rules

		private static void AddObjectAuthorizationRules()
		{
			// TODO: add object-level authorization rules
		}

		#endregion

		#region Defensive Fire

		public static readonly PropertyInfo<bool> CanDefensiveFireProperty = RegisterProperty<bool>(c => c.CanDefensiveFire);
		/// <Summary>
		/// Gets or sets the CanDefensiveFire value.
		/// </Summary>
		public bool CanDefensiveFire
		{
			get { return GetProperty(CanDefensiveFireProperty); }
			set { SetProperty(CanDefensiveFireProperty, value); }
		}

		public static readonly PropertyInfo<bool> DefensiveFireProperty = RegisterProperty<bool>(c => c.DefensiveFire);
		/// <Summary>
		/// Gets or sets the DefensiveFire value.
		/// </Summary>
		public bool DefensiveFire
		{
			get { return GetProperty(DefensiveFireProperty); }
			set { SetProperty(DefensiveFireProperty, value); }
		}

		public static readonly PropertyInfo<bool> SimultaneousDefensiveFireProperty = RegisterProperty<bool>(c => c.SimultaneousDefensiveFire);
		/// <Summary>
		/// Gets or sets the SimultaneousDefensiveFire value.
		/// </Summary>
		public bool SimultaneousDefensiveFire
		{
			get { return GetProperty(SimultaneousDefensiveFireProperty); }
			set { SetProperty(SimultaneousDefensiveFireProperty, value); }
		}

		public static readonly PropertyInfo<FireAttack> DefensiveFireAttackProperty = RegisterProperty<FireAttack>(c => c.DefensiveFireAttack);
		/// <Summary>
		/// Gets or sets the DefensiveFireAttack value.
		/// </Summary>
		public FireAttack DefensiveFireAttack
		{
			get { return GetProperty(DefensiveFireAttackProperty); }
			set { SetProperty(DefensiveFireAttackProperty, value); }
		}

		#endregion

		#region Rate

		public static readonly PropertyInfo<RelativeRate> ModifiedRateProperty = RegisterProperty<RelativeRate>(c => c.ModifiedRate);
		/// <Summary>
		/// Gets or sets the ModifiedRate value.
		/// </Summary>
		public RelativeRate ModifiedRate
		{
			get { return GetProperty(ModifiedRateProperty); }
			set { SetProperty(ModifiedRateProperty, value); }
		}

		public static readonly PropertyInfo<bool> PartialBroadsideProperty = RegisterProperty<bool>(c => c.PartialBroadside);
		/// <Summary>
		/// Gets or sets the PartialBroadside value.
		/// </Summary>
		public bool PartialBroadside
		{
			get { return GetProperty(PartialBroadsideProperty); }
			set { SetProperty(PartialBroadsideProperty, value); }
		}

		public static readonly PropertyInfo<bool> DuringTackProperty = RegisterProperty<bool>(c => c.DuringTack);
		/// <Summary>
		/// Gets or sets the DuringTack value.
		/// </Summary>
		public bool DuringTack
		{
			get { return GetProperty(DuringTackProperty); }
			set { SetProperty(DuringTackProperty, value); }
		}

		#endregion

		#region Fire power

		public static readonly PropertyInfo<FirePower> BaseFirePowerProperty = RegisterProperty<FirePower>(c => c.BaseFirePower);
		/// <Summary>
		/// Gets or sets the BaseFirePower value.
		/// </Summary>
		public FirePower BaseFirePower
		{
			get { return GetProperty(BaseFirePowerProperty); }
			set { SetProperty(BaseFirePowerProperty, value); }
		}

		public static readonly PropertyInfo<FirePower> ModifiedFirePowerProperty = RegisterProperty<FirePower>(c => c.ModifiedFirePower);
		/// <Summary>
		/// Gets or sets the ModifiedFirePower value.
		/// </Summary>
		public FirePower ModifiedFirePower
		{
			get { return GetProperty(ModifiedFirePowerProperty); }
			set { SetProperty(ModifiedFirePowerProperty, value); }
		}

		public static readonly PropertyInfo<WeatherGauge> WeatherGaugeProperty = RegisterProperty<WeatherGauge>(c => c.WeatherGauge);
		/// <Summary>
		/// Gets or sets the WeatherGauge value.
		/// </Summary>
		public WeatherGauge WeatherGauge
		{
			get { return GetProperty(WeatherGaugeProperty); }
			set { SetProperty(WeatherGaugeProperty, value); }
		}

		public static readonly PropertyInfo<TargetArea> TargettingProperty = RegisterProperty<TargetArea>(c => c.Targetting);
		/// <Summary>
		/// Gets or sets the Targetting value.
		/// </Summary>
		public TargetArea Targetting
		{
			get { return GetProperty(TargettingProperty); }
			set { SetProperty(TargettingProperty, value); }
		}

		#endregion

		#region Damage Results

		public static readonly PropertyInfo<int> DieRollProperty = RegisterProperty<int>(c => c.DieRoll);
		/// <Summary>
		/// Gets or sets the DieRoll value.
		/// </Summary>
		public int DieRoll
		{
			get { return GetProperty(DieRollProperty); }
			set { SetProperty(DieRollProperty, value); }
		}

		public static readonly PropertyInfo<bool> OutsideBroadsideArcProperty = RegisterProperty<bool>(c => c.OutsideBroadsideArc);
		/// <Summary>
		/// Gets or sets the OutsideBroadsideArc value.
		/// </Summary>
		public bool OutsideBroadsideArc
		{
			get { return GetProperty(OutsideBroadsideArcProperty); }
			set { SetProperty(OutsideBroadsideArcProperty, value); }
		}

		public static readonly PropertyInfo<bool> DownOwnRakeLineProperty = RegisterProperty<bool>(c => c.DownOwnRakeLine);
		/// <Summary>
		/// Gets or sets the DownOwnRakeLine value.
		/// </Summary>
		public bool DownOwnRakeLine
		{
			get { return GetProperty(DownOwnRakeLineProperty); }
			set { SetProperty(DownOwnRakeLineProperty, value); }
		}

		public static readonly PropertyInfo<int> ModifiedDieRollProperty = RegisterProperty<int>(c => c.ModifiedDieRoll);
		/// <Summary>
		/// Gets or sets the ModifiedDieRoll value.
		/// </Summary>
		public int ModifiedDieRoll
		{
			get { return GetProperty(ModifiedDieRollProperty); }
			set { SetProperty(ModifiedDieRollProperty, value); }
		}

		public static readonly PropertyInfo<Damage> DamageProperty = RegisterProperty<Damage>(c => c.Damage);
		/// <Summary>
		/// Gets or sets the Damage value.
		/// </Summary>
		public Damage Damage
		{
			get { return GetProperty(DamageProperty); }
			set { SetProperty(DamageProperty, value); }
		}

		#endregion

		#region Factory Methods

		public static FireAttack NewFireAttack(BattleShip firing)
		{
			return DataPortal.Create<FireAttack>(firing);
		}

		private static FireAttack NewDefensiveFireAttack(BattleShip target)
		{
			return DataPortal.CreateChild<FireAttack>(target);
		}

		#endregion

		#region Data Portal

		private void DataPortal_Create(BattleShip firing)
		{
			LoadProperty<BattleShip>(FiringShipProperty, firing);
			LoadProperty<bool>(CanDefensiveFireProperty, true);
			LoadProperty<FireAttack>(DefensiveFireAttackProperty, FireAttack.NewDefensiveFireAttack(firing));
		}

		private void Child_Create(BattleShip target)
		{
			LoadProperty<BattleShip>(TargetShipProperty, target);
			LoadProperty<bool>(CanDefensiveFireProperty, false);
		}
		

		#endregion
	}
}
