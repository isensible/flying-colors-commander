using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using Csla.Rules;
using Csla.Core;
using System.ComponentModel;

namespace FlyingColors
{
	[Serializable]
	public class FireAttack : BusinessBase<FireAttack>
	{
		public static readonly PropertyInfo<BattleShip> FiringShipProperty = RegisterProperty<BattleShip>(c => c.FiringShip);
		/// <Summary>
		/// Gets or sets the FiringShip value.
		/// </Summary>
		[Category("Attacker")]
		[ReadOnly(true)]
		[Browsable(true)]
		public BattleShip FiringShip
		{
			get { return GetProperty(FiringShipProperty); }
			set { SetProperty(FiringShipProperty, value); }
		}

		public static readonly PropertyInfo<BattleShip> TargetShipProperty = RegisterProperty<BattleShip>(c => c.TargetShip);
		/// <Summary>
		/// Gets or sets the TargetShip value.
		/// </Summary>
		[Category("Defender")]
		[ReadOnly(false)]
		[Browsable(true)]
		public BattleShip TargetShip
		{
			get { return GetProperty(TargetShipProperty); }
			set { SetProperty(TargetShipProperty, value); }
		}

		public static PropertyInfo<Broadside> FiringFromProperty = RegisterProperty<Broadside>(c => c.FiringFrom);
		/// <summary>
		/// The broadside from which the FiringShip is firing.
		/// </summary>
		[Category("Broadside")]
		[ReadOnly(false)]
		[Browsable(true)]
		public Broadside FiringFrom
		{
			get { return GetProperty(FiringFromProperty); }
			set { SetProperty(FiringFromProperty, value); }
		}

		public static PropertyInfo<RakeType> RakeTypeProperty = RegisterProperty<RakeType>(c => c.RakeType);
		[Category("Broadside")]
		[ReadOnly(false)]
		[Browsable(true)]
		public RakeType RakeType
		{
			get { return GetProperty(RakeTypeProperty); }
			set { SetProperty(RakeTypeProperty, value); }
		}

		public static PropertyInfo<bool> IsRakeProperty = RegisterProperty<bool>(c => c.IsRake);
		[Category("Broadside")]
		[ReadOnly(false)]
		[Browsable(false)]
		public bool IsRake
		{
			get { return GetProperty(IsRakeProperty); }
			set { SetProperty(IsRakeProperty, value); }
		}

		public static readonly PropertyInfo<int> RangeProperty = RegisterProperty<int>(c => c.Range);
		/// <Summary>
		/// Gets or sets the Range value.
		/// </Summary>
		[Category("Defender")]
		[ReadOnly(false)]
		[Browsable(true)]
		public int Range
		{
			get { return GetProperty(RangeProperty); }
			set { SetProperty(RangeProperty, value); }
		}

		public static readonly PropertyInfo<bool> IsPointBlankProperty = RegisterProperty<bool>(c => c.IsPointBlank);
		/// <Summary>
		/// Gets or sets the IsPointBlank value.
		/// </Summary>
		[Category("Defender")]
		[ReadOnly(true)]	
		[Browsable(false)]
		public bool IsPointBlank
		{
			get { return GetProperty(IsPointBlankProperty); }
			set { SetProperty(IsPointBlankProperty, value); }
		}

		private class RangeRule : BusinessRule
		{
			public RangeRule()
				: base(RangeProperty)
			{
				AffectedProperties.Add(IsPointBlankProperty);
				AffectedProperties.Add(BaseFirePowerProperty);
			}

			protected override void Execute(RuleContext context)
			{
				var fireAttack = (FireAttack)context.Target;
				if (fireAttack.Range < 0 || fireAttack.Range > 10)
				{
					context.AddErrorResult("Range must be integer between 0 and 10.");
				}
				else if (fireAttack.Range == 0)
				{
					context.AddOutValue(IsPointBlankProperty, true);
				}
				else
				{
					context.AddOutValue(IsPointBlankProperty, false);
				}
				if (fireAttack.Range >= 0 && fireAttack.Range <= 10)
				{
					context.AddOutValue(BaseFirePowerProperty,
						FirePower.GetBase(fireAttack.ModifiedRate, fireAttack.Range));
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


		#region Validation Rules

		protected override void AddBusinessRules()
		{
			base.AddBusinessRules();

			BusinessRules.AddRule(new RangeRule());
			BusinessRules.AddRule(new IsPointBlankRule());
			BusinessRules.AddRule(new ModifiedRateRule());
			BusinessRules.AddRule(new PartialBroadsideRule());
			BusinessRules.AddRule(new DuringTackRule());
			BusinessRules.AddRule(new BaseFirePowerRule());
			BusinessRules.AddRule(new DieRollRule());

		}

		#endregion

		#region Defensive Fire

		public static readonly PropertyInfo<bool> CanDefensiveFireProperty = RegisterProperty<bool>(c => c.CanDefensiveFire);
		/// <Summary>
		/// Gets or sets the CanDefensiveFire value.
		/// </Summary>
		[Browsable(false)]
		public bool CanDefensiveFire
		{
			get { return GetProperty(CanDefensiveFireProperty); }
			private set { LoadProperty(CanDefensiveFireProperty, value); }
		}

		public static readonly PropertyInfo<bool> DefensiveFireProperty = RegisterProperty<bool>(c => c.DefensiveFire);
		/// <Summary>
		/// Gets or sets the DefensiveFire value.
		/// </Summary>
		[Browsable(false)]
		public bool DefensiveFire
		{
			get { return GetProperty(DefensiveFireProperty); }
			set { SetProperty(DefensiveFireProperty, value); }
		}

		public static readonly PropertyInfo<bool> SimultaneousDefensiveFireProperty = RegisterProperty<bool>(c => c.DefensiveFireSimultaneousDamage);
		/// <Summary>
		/// Gets or sets the DefensiveFireSimultaneousDamage value.
		/// </Summary>
		[Browsable(false)]
		public bool DefensiveFireSimultaneousDamage
		{
			get { return GetProperty(SimultaneousDefensiveFireProperty); }
			private set { SetProperty(SimultaneousDefensiveFireProperty, value); }
		}

		public static readonly PropertyInfo<FireAttack> DefensiveFireAttackProperty = RegisterProperty<FireAttack>(c => c.DefensiveFireAttack);
		/// <Summary>
		/// Gets or sets the DefensiveFireAttack value.
		/// </Summary>
		[Browsable(false)]
		public FireAttack DefensiveFireAttack
		{
			get { return GetProperty(DefensiveFireAttackProperty); }
			set { SetProperty(DefensiveFireAttackProperty, value); }
		}

		#endregion

		#region Rate

		/// <summary>
		/// Calculates the modified rate of the firing ship, excluding shifts for
		/// partial broadside and tacking which are handled by business rules.
		/// </summary>
		private void CalculateModifiedRate()
		{
			RelativeRate rate = FiringShip.CurrentRate;
			for (int shifts = 0; shifts < FiringShip.HullHitRateShifts; ++shifts)
			{
				rate = rate.ShiftUp();
			}
			if (FiringShip.FullSails)
			{
				rate = rate.ShiftUp();
			}
			if (FiringShip.Dismasted)
			{
				rate = rate.ShiftUp();
			}
			if (FiringShip.OnFire)
			{
				rate = rate.ShiftUp();
			}
			ModifiedRate = rate;
		}

		public static readonly PropertyInfo<RelativeRate> ModifiedRateProperty = RegisterProperty<RelativeRate>(c => c.ModifiedRate);
		/// <Summary>
		/// Gets or sets the ModifiedRate value.
		/// </Summary>
		[Category("Rate")]
		[ReadOnly(true)]
		[Browsable(true)]
		public RelativeRate ModifiedRate
		{
			get { return GetProperty(ModifiedRateProperty); }
			set { SetProperty(ModifiedRateProperty, value); }
		}

		private class ModifiedRateRule : BusinessRule
		{
			public ModifiedRateRule()
				: base(ModifiedRateProperty)
			{
				AffectedProperties.Add(BaseFirePowerProperty);
			}

			protected override void Execute(RuleContext context)
			{
				var fireAttack = (FireAttack)context.Target;
				var baseFirePower = FirePower.GetBase(fireAttack.ModifiedRate, fireAttack.Range);
				context.AddOutValue(BaseFirePowerProperty, baseFirePower);
			}
		}

		public static readonly PropertyInfo<bool> PartialBroadsideProperty = RegisterProperty<bool>(c => c.PartialBroadside);
		/// <Summary>
		/// Gets or sets the PartialBroadside value.
		/// </Summary>
		[Category("Broadside")]
		[ReadOnly(false)]
		[Browsable(true)]
		public bool PartialBroadside
		{
			get { return GetProperty(PartialBroadsideProperty); }
			set { SetProperty(PartialBroadsideProperty, value); }
		}

		private class PartialBroadsideRule : BusinessRule
		{
			public PartialBroadsideRule()
				: base(PartialBroadsideProperty)
			{
				AffectedProperties.Add(ModifiedRateProperty);
			}

			protected override void Execute(RuleContext context)
			{
				var fireAttack = (FireAttack)context.Target;
				if (fireAttack.PartialBroadside)
				{
					context.AddOutValue(ModifiedRateProperty, fireAttack.ModifiedRate.ShiftUp());
				}
				else
				{
					context.AddOutValue(ModifiedRateProperty, fireAttack.ModifiedRate.ShiftDown());
				}
			}
		}

		public static readonly PropertyInfo<bool> DuringTackProperty = RegisterProperty<bool>(c => c.DuringTack);
		/// <Summary>
		/// Gets or sets the DuringTack value.
		/// </Summary>
		[Category("Attacker")]
		[ReadOnly(false)]
		[Browsable(true)]
		public bool DuringTack
		{
			get { return GetProperty(DuringTackProperty); }
			set { SetProperty(DuringTackProperty, value); }
		}

		private class DuringTackRule : BusinessRule
		{
			public DuringTackRule()
				: base(DuringTackProperty)
			{
				AffectedProperties.Add(ModifiedRateProperty);
			}

			protected override void Execute(RuleContext context)
			{
				var fireAttack = (FireAttack)context.Target;
				if (fireAttack.DuringTack)
				{
					context.AddOutValue(ModifiedRateProperty, fireAttack.ModifiedRate.ShiftUp());
				}
				else
				{
					context.AddOutValue(ModifiedRateProperty, fireAttack.ModifiedRate.ShiftDown());
				}
			}
		}

		#endregion

		#region Fire power

		public static readonly PropertyInfo<FirePower> BaseFirePowerProperty = RegisterProperty<FirePower>(c => c.BaseFirePower);
		/// <Summary>
		/// Gets or sets the BaseFirePower value.
		/// </Summary>
		[Category("Fire Power")]
		[ReadOnly(true)]
		[Browsable(true)]
		public FirePower BaseFirePower
		{
			get { return GetProperty(BaseFirePowerProperty); }
			set { SetProperty(BaseFirePowerProperty, value); }
		}

		private class BaseFirePowerRule : BusinessRule
		{
			public BaseFirePowerRule()
				: base(BaseFirePowerProperty)
			{
				AffectedProperties.Add(ModifiedFirePowerProperty);
			}

			protected override void Execute(RuleContext context)
			{
				var fireAttack = (FireAttack)context.Target;
				FirePower modifiedFirePower = fireAttack.BaseFirePower.CopySelf();
				if (fireAttack.ModifiedRate.IsWhite &&
					fireAttack.BaseFirePower.IsShaded)
				{
					modifiedFirePower = modifiedFirePower.Increment();
				}
				if (fireAttack.ModifiedRate.IsBlack &&
					fireAttack.BaseFirePower.IsShaded)
				{
					modifiedFirePower = modifiedFirePower.Decrement();
				}
				modifiedFirePower = modifiedFirePower.IncrementBy(fireAttack.FiringShip.Audacity);
				if (fireAttack.IsPointBlank)
				{
					modifiedFirePower = modifiedFirePower.IncrementBy(2);
				}
				if (fireAttack.FiringFrom == Broadside.Port &&
					!fireAttack.FiringShip.FirstPortFired)
				{
					modifiedFirePower = modifiedFirePower.IncrementBy(2);
				}
				else if (fireAttack.FiringFrom == Broadside.Starboard &&
					!fireAttack.FiringShip.FirstStarboardFired)
				{
					modifiedFirePower = modifiedFirePower.IncrementBy(2);
				}
				modifiedFirePower = CalculateCarronadeBonus(fireAttack, modifiedFirePower);
				context.AddOutValue(ModifiedFirePowerProperty, modifiedFirePower);
			}

			private FirePower CalculateCarronadeBonus(FireAttack fireAttack, FirePower modifiedFirePower)
			{
				bool earlyYears = (fireAttack.FiringShip.Year >= 1779) && (fireAttack.FiringShip.Year <= 1786);
				bool laterYears = (fireAttack.FiringShip.Year >= 1787) && (fireAttack.FiringShip.Year <= 1826);
				bool rangeLessThanOrEqualThree = (fireAttack.Range <= 3);
				bool rangeLessThanOrEqualOne = (fireAttack.Range <= 1);
				switch (fireAttack.FiringShip.Nationality)
				{
					case Nationality.British:
						{
							if (rangeLessThanOrEqualThree)
							{
								modifiedFirePower = modifiedFirePower.Increment();
							}
							break;
						}
					case Nationality.American:
						{
							if ((rangeLessThanOrEqualThree && laterYears) || rangeLessThanOrEqualOne)
							{
								modifiedFirePower = modifiedFirePower.Increment();
							}
							break;
						}
					case Nationality.French:
					case Nationality.Dutch:
						{
							if (rangeLessThanOrEqualThree && laterYears)
							{
								modifiedFirePower = modifiedFirePower.Increment();
							}
							break;
						}
					case Nationality.Spanish:
						{
							if (rangeLessThanOrEqualOne && laterYears)
							{
								modifiedFirePower = modifiedFirePower.Increment();
							}
							break;
						}
					default:
						{
							break;
						}
				}
				if (fireAttack.ModifiedRate.IsHexagonal && rangeLessThanOrEqualThree)
				{
					modifiedFirePower = modifiedFirePower.Increment();
				}
				if (fireAttack.ModifiedRate.IsSquare && rangeLessThanOrEqualThree)
				{
					modifiedFirePower = modifiedFirePower.IncrementBy(2);
				}
				return modifiedFirePower;
			}
		}

		public static readonly PropertyInfo<FirePower> ModifiedFirePowerProperty = RegisterProperty<FirePower>(c => c.ModifiedFirePower);
		/// <Summary>
		/// Gets or sets the ModifiedFirePower value.
		/// </Summary>
		[Category("Fire Power")]
		[ReadOnly(true)]
		[Browsable(true)]
		public FirePower ModifiedFirePower
		{
			get { return GetProperty(ModifiedFirePowerProperty); }
			set { SetProperty(ModifiedFirePowerProperty, value); }
		}

		#endregion

		#region Damage Results

		public static readonly PropertyInfo<int> DieRollProperty = RegisterProperty<int>(c => c.DieRoll);
		/// <Summary>
		/// Gets or sets the DieRoll value.
		/// </Summary>
		[Category("Hit Results")]
		[ReadOnly(false)]
		[Browsable(true)]
		public int DieRoll
		{
			get { return GetProperty(DieRollProperty); }
			set { SetProperty(DieRollProperty, value); }
		}

		private class DieRollRule : BusinessRule
		{
			public DieRollRule()
				: base(DieRollProperty)
			{
				AffectedProperties.Add(ModifiedDieRollProperty);
				AffectedProperties.Add(ModifiedDieRollExcessProperty);
				AffectedProperties.Add(DamageProperty);
				AffectedProperties.Add(DamageExcessProperty);
				AffectedProperties.Add(ChanceOfFireProperty);
				AffectedProperties.Add(TotalHullDamageProperty);
				AffectedProperties.Add(TotalRiggingDamageProperty);
				AffectedProperties.Add(TotalMarineDamageProperty);
				AffectedProperties.Add(TotalDamageProperty);
			}

			protected override void Execute(RuleContext context)
			{
				var fireAttack = (FireAttack)context.Target;
				int modifier = 0;
				modifier = ApplyRiggingModifiers(fireAttack, modifier);
				modifier = ApplyHullModifiers(fireAttack, modifier);
				modifier = ApplyAnchoredModifier(fireAttack, modifier);
				// TODO: battery firing!
				modifier = ApplyFiringArcModifiers(fireAttack, modifier);
				modifier = EnsureModifierLessThanOrEqualToFive(modifier);

				int modifiedDieRoll;
				int excessDieRoll;
				CalculateModifiedDieRoll(fireAttack, modifier, out modifiedDieRoll, out excessDieRoll);
				context.AddOutValue(ModifiedDieRollProperty, modifiedDieRoll);
				context.AddOutValue(ModifiedDieRollExcessProperty, excessDieRoll);

				Damage damage = new Damage("-");
				Damage excessDamage = new Damage("-");
				CalculateDamage(fireAttack, modifiedDieRoll, excessDieRoll, out damage, out excessDamage);
				context.AddOutValue(DamageProperty, damage);
				context.AddOutValue(DamageExcessProperty, excessDamage);

				bool chanceOfFire = damage.ChanceOfFire || excessDamage.ChanceOfFire;
				context.AddOutValue(ChanceOfFireProperty, chanceOfFire);

				decimal rakeMultiplier = 1m;
				if (fireAttack.IsRake)
				{
					rakeMultiplier = fireAttack.RakeType == RakeType.Bow ? 1.5m : 2m;
				}
				int pointBlankMarineDamageModifier = fireAttack.IsPointBlank ? 2 : 1;

				int totalHullDamage = (int)((decimal)(damage.Hull + excessDamage.Hull) * rakeMultiplier);
				int totalRiggingDamage = damage.Rigging + excessDamage.Rigging;
				int totalMarineDamage = (damage.Marines + excessDamage.Marines) * pointBlankMarineDamageModifier;
				int totalDamage = totalHullDamage + totalRiggingDamage;
				context.AddOutValue(TotalHullDamageProperty, totalHullDamage);
				context.AddOutValue(TotalRiggingDamageProperty, totalRiggingDamage);
				context.AddOutValue(TotalMarineDamageProperty, totalMarineDamage);
				context.AddOutValue(TotalDamageProperty, totalDamage);
			}

			private static void CalculateDamage(FireAttack fireAttack, int modifiedDieRoll, int excessDieRoll, out Damage damage, out Damage excessDamage)
			{
				damage = new Damage("-");
				excessDamage = new Damage("-");
				if (fireAttack.TargetShip.IsSmallVessel)
				{
					if (fireAttack.Targetting == TargetArea.Rigging)
					{
						damage = HitResults.GetSmallVesselRiggingDamage(
							fireAttack.ModifiedFirePower, modifiedDieRoll);
						if (excessDieRoll >= 0)
						{
							excessDamage = HitResults.GetSmallVesselHullDamage(
								fireAttack.ModifiedFirePower, excessDieRoll);
						}
					}
					if (fireAttack.Targetting == TargetArea.Hull)
					{
						damage = HitResults.GetSmallVesselHullDamage(
							fireAttack.ModifiedFirePower, modifiedDieRoll);
						if (excessDieRoll >= 0)
						{
							excessDamage = HitResults.GetSmallVesselRiggingDamage(
								fireAttack.ModifiedFirePower, excessDieRoll);
						}
					}
				}
				else
				{
					if (fireAttack.Targetting == TargetArea.Rigging)
					{
						damage = HitResults.GetRiggingDamage(
							fireAttack.ModifiedFirePower, modifiedDieRoll);
						if (excessDieRoll >= 0)
						{
							excessDamage = HitResults.GetHullDamage(
								fireAttack.ModifiedFirePower, excessDieRoll);
						}
					}
					if (fireAttack.Targetting == TargetArea.Hull)
					{
						damage = HitResults.GetHullDamage(
							fireAttack.ModifiedFirePower, modifiedDieRoll);
						if (excessDieRoll >= 0)
						{
							excessDamage = HitResults.GetRiggingDamage(
								fireAttack.ModifiedFirePower, excessDieRoll);
						}
					}
				}
			}

			private static void CalculateModifiedDieRoll(FireAttack fireAttack, int modifier, out int modifiedDieRoll, out int excessDieRoll)
			{
				modifiedDieRoll = fireAttack.DieRoll + modifier;
				excessDieRoll = -1;
				if (modifiedDieRoll > 12)
				{
					int div = Math.DivRem(modifiedDieRoll, 12, out excessDieRoll);
					modifiedDieRoll = 12;
				}
				else if (modifiedDieRoll < -1)
				{
					modifiedDieRoll = -1;
				}
			}

			private static int EnsureModifierLessThanOrEqualToFive(int modifier)
			{
				if (modifier > 5)
				{
					modifier = 5;
				}
				return modifier;
			}

			private static int ApplyFiringArcModifiers(FireAttack fireAttack, int modifier)
			{
				if (fireAttack.OutsideBroadsideArc)
				{
					modifier--;
				}
				if (fireAttack.DownOwnRakeLine)
				{
					modifier -= 3;
				}
				return modifier;
			}

			private static int ApplyAnchoredModifier(FireAttack fireAttack, int modifier)
			{
				if (fireAttack.FiringShip.Anchored)
				{
					modifier += 2;
				}
				return modifier;
			}

			private static int ApplyHullModifiers(FireAttack fireAttack, int modifier)
			{
				if (fireAttack.Targetting == TargetArea.Hull)
				{
					if (fireAttack.FiringShip.Nationality == Nationality.British)
					{
						modifier++;
					}
					if (fireAttack.WeatherGauge == WeatherGauge.Leeward)
					{
						modifier++;
					}
					if (fireAttack.WeatherGauge == WeatherGauge.Windward)
					{
						modifier--;
					}
				}
				return modifier;
			}

			private static int ApplyRiggingModifiers(FireAttack fireAttack, int modifier)
			{
				if (fireAttack.Targetting == TargetArea.Rigging)
				{
					if (fireAttack.FiringShip.Nationality == Nationality.French)
					{
						modifier++;
					}
					if (fireAttack.WeatherGauge == WeatherGauge.Windward)
					{
						modifier++;
					}
					if (fireAttack.WeatherGauge == WeatherGauge.Leeward)
					{
						modifier--;
					}
					if (fireAttack.TargetShip.FullSails)
					{
						modifier += 2;
					}
					if (fireAttack.TargetShip.Weather == Weather.Calm)
					{
						modifier -= 2;
					}
				}
				return modifier;
			}
		}

		public static readonly PropertyInfo<WeatherGauge> WeatherGaugeProperty = RegisterProperty<WeatherGauge>(c => c.WeatherGauge);
		/// <Summary>
		/// Gets or sets the WeatherGauge value.
		/// </Summary>
		[Category("Defender")]
		[ReadOnly(false)]
		[Browsable(true)]
		public WeatherGauge WeatherGauge
		{
			get { return GetProperty(WeatherGaugeProperty); }
			set { SetProperty(WeatherGaugeProperty, value); }
		}

		public static readonly PropertyInfo<TargetArea> TargettingProperty = RegisterProperty<TargetArea>(c => c.Targetting);
		/// <Summary>
		/// Gets or sets the Targetting value.
		/// </Summary>
		[Category("Defender")]
		[ReadOnly(false)]
		[Browsable(true)]
		public TargetArea Targetting
		{
			get { return GetProperty(TargettingProperty); }
			set { SetProperty(TargettingProperty, value); }
		}

		public static readonly PropertyInfo<bool> OutsideBroadsideArcProperty = RegisterProperty<bool>(c => c.OutsideBroadsideArc);
		/// <Summary>
		/// Gets or sets the OutsideBroadsideArc value.
		/// </Summary>
		[Browsable(false)] // until small vessel support
		public bool OutsideBroadsideArc
		{
			get { return GetProperty(OutsideBroadsideArcProperty); }
			set { SetProperty(OutsideBroadsideArcProperty, value); }
		}

		public static readonly PropertyInfo<bool> DownOwnRakeLineProperty = RegisterProperty<bool>(c => c.DownOwnRakeLine);
		/// <Summary>
		/// Gets or sets the DownOwnRakeLine value.
		/// </Summary>
		[Browsable(false)]
		public bool DownOwnRakeLine
		{
			get { return GetProperty(DownOwnRakeLineProperty); }
			set { SetProperty(DownOwnRakeLineProperty, value); }
		}

		public static readonly PropertyInfo<int> ModifiedDieRollProperty = RegisterProperty<int>(c => c.ModifiedDieRoll);
		/// <Summary>
		/// Gets or sets the ModifiedDieRoll value.
		/// </Summary>
		[Category("Hit Results")]
		[ReadOnly(true)]
		[Browsable(true)]
		public int ModifiedDieRoll
		{
			get { return GetProperty(ModifiedDieRollProperty); }
			set { SetProperty(ModifiedDieRollProperty, value); }
		}

		public static readonly PropertyInfo<Damage> DamageProperty = RegisterProperty<Damage>(c => c.Damage);
		/// <Summary>
		/// Gets or sets the Damage value.
		/// </Summary>
		[Category("Hit Results")]
		[ReadOnly(true)]
		[Browsable(true)]
		public Damage Damage
		{
			get { return GetProperty(DamageProperty); }
			set { SetProperty(DamageProperty, value); }
		}

		public static PropertyInfo<int> ModifiedDieRollExcessProperty = RegisterProperty<int>(c => c.ModifiedDieRollExcess);
		[Browsable(false)]
		public int ModifiedDieRollExcess
		{
			get { return GetProperty(ModifiedDieRollExcessProperty); }
			set { SetProperty(ModifiedDieRollExcessProperty, value); }
		}

		public static PropertyInfo<Damage> DamageExcessProperty = RegisterProperty<Damage>(c => c.DamageExcess);
		[Category("Hit Results")]
		[ReadOnly(true)]
		[Browsable(true)]
		public Damage DamageExcess
		{
			get { return GetProperty(DamageExcessProperty); }
			set { SetProperty(DamageExcessProperty, value); }
		}

		public static readonly PropertyInfo<int> TotalHullDamageProperty = RegisterProperty<int>(c => c.TotalHullDamage);
		/// <Summary>
		/// Gets or sets the TotalHullDamage value.
		/// </Summary>
		[Browsable(false)]
		public int TotalHullDamage
		{
			get { return GetProperty(TotalHullDamageProperty); }
			set { SetProperty(TotalHullDamageProperty, value); }
		}

		public static readonly PropertyInfo<int> TotalRiggingDamageProperty = RegisterProperty<int>(c => c.TotalRiggingDamage);
		/// <Summary>
		/// Gets or sets the TotalRiggingDamage value.
		/// </Summary>
		[Browsable(false)]
		public int TotalRiggingDamage
		{
			get { return GetProperty(TotalRiggingDamageProperty); }
			set { SetProperty(TotalRiggingDamageProperty, value); }
		}

		public static PropertyInfo<int> TotalMarineDamageProperty = RegisterProperty<int>(c => c.TotalMarineDamage);
		[Browsable(false)]
		public int TotalMarineDamage
		{
			get { return GetProperty(TotalMarineDamageProperty); }
			set { SetProperty(TotalMarineDamageProperty, value); }
		}

		public static readonly PropertyInfo<int> TotalDamageProperty = RegisterProperty<int>(c => c.TotalDamage);
		/// <Summary>
		/// Gets or sets the TotalDamage value.
		/// </Summary>
		[Browsable(false)]
		public int TotalDamage
		{
			get { return GetProperty(TotalDamageProperty); }
			set { SetProperty(TotalDamageProperty, value); }
		}

		public void ApplyDamage()
		{
			TargetShip.HullHits += TotalHullDamage;
			TargetShip.RiggingHits += TotalRiggingDamage;
			TargetShip.MarineHits += TotalMarineDamage;
			TargetShip.OnFire = TargetShip.OnFire ? true : SetTargetShipOnFire;
		}

		public static PropertyInfo<bool> ChanceOfFireProperty = RegisterProperty<bool>(c => c.ChanceOfFire);
		[Browsable(false)]
		public bool ChanceOfFire
		{
			get { return GetProperty(ChanceOfFireProperty); }
			set { SetProperty(ChanceOfFireProperty, value); }
		}

		public static PropertyInfo<int> ChanceOfFireDieRollProperty = RegisterProperty<int>(c => c.ChanceOfFireDieRoll);
		[Browsable(false)]
		public int ChanceOfFireDieRoll
		{
			get { return GetProperty(ChanceOfFireDieRollProperty); }
			set { SetProperty(ChanceOfFireDieRollProperty, value); }
		}

		public static PropertyInfo<bool> SetTargetShipOnFireProperty = RegisterProperty<bool>(c => c.SetTargetShipOnFire);
		[Category("Hit Results")]
		[ReadOnly(false)]
		[Browsable(true)]
		public bool SetTargetShipOnFire
		{
			get { return GetProperty(SetTargetShipOnFireProperty); }
			set { SetProperty(SetTargetShipOnFireProperty, value); }
		}

		#endregion

		#region Factory Methods

		public static FireAttack NewFireAttack(BattleShip firing)
		{
			return DataPortal.Create<FireAttack>(firing);
		}

		#endregion

		#region Data Portal

		private void DataPortal_Create(BattleShip firing)
		{
			LoadProperty<BattleShip>(FiringShipProperty, firing);
			CalculateModifiedRate();
		}

		#endregion
	}
}
