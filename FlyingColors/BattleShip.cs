using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using Csla.Rules;

namespace FlyingColors
{
	[Serializable]
	public class BattleShip : BusinessBase<BattleShip>
	{
		#region ID
		public static PropertyInfo<long> BattleShipIdProperty = RegisterProperty<long>(c => c.BattleShipId);
		public long BattleShipId
		{
			get { return GetProperty(BattleShipIdProperty); }
			private set { LoadProperty(BattleShipIdProperty, value); }
		}
		#endregion

		#region Fleet Properties

		public static PropertyInfo<int> AudacityProperty = RegisterProperty<int>(c => c.Audacity);
		public int Audacity
		{
			get { return GetProperty(AudacityProperty); }
			private set { LoadProperty(AudacityProperty, value); }
		}

		#endregion

		#region Commanders

		public static PropertyInfo<BattleShipCommanderList> CommandersProperty = RegisterProperty<BattleShipCommanderList>(c => c.Commanders);
		public BattleShipCommanderList Commanders
		{
			get { return GetProperty(CommandersProperty); }
			set { SetProperty(CommandersProperty, value); }
		}

		#endregion

		#region Ship Properties

		public static PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
		public string Name
		{
			get { return GetProperty(NameProperty); }
			private set { LoadProperty(NameProperty, value); }
		}

		#region Nationality and Victory Points
		public static PropertyInfo<Nationality> NationalityProperty = RegisterProperty<Nationality>(c => c.Nationality);
		public Nationality Nationality
		{
			get { return GetProperty(NationalityProperty); }
			private set { LoadProperty(NationalityProperty, value); }
		}

		public static PropertyInfo<int> VictoryPointsProperty = RegisterProperty<int>(c => c.VictoryPoints);
		public int VictoryPoints
		{
			get { return GetProperty(VictoryPointsProperty); }
			private set { LoadProperty(VictoryPointsProperty, value); }
		}
		#endregion

		#region Relative Rate
		public static PropertyInfo<RelativeRate> RateProperty = RegisterProperty<RelativeRate>(c => c.Rate);
		public RelativeRate Rate
		{
			get { return GetProperty(RateProperty); }
			private set { LoadProperty(RateProperty, value); }
		}

		public static PropertyInfo<RelativeRate> RateDamagedProperty = RegisterProperty<RelativeRate>(c => c.RateDamaged);
		public RelativeRate RateDamaged
		{
			get { return GetProperty(RateDamagedProperty); }
			private set { LoadProperty(RateDamagedProperty, value); }
		}
		#endregion

		#region Damage Capacity
		public static PropertyInfo<int> DamageCapacityProperty = RegisterProperty<int>(c => c.DamageCapacity);
		public int DamageCapacity
		{
			get { return GetProperty(DamageCapacityProperty); }
			private set { LoadProperty(DamageCapacityProperty, value); }
		}

		public static PropertyInfo<int> DamageCapacityDamagedProperty = RegisterProperty<int>(c => c.DamageCapacityDamaged);
		public int DamageCapacityDamaged
		{
			get { return GetProperty(DamageCapacityDamagedProperty); }
			private set { LoadProperty(DamageCapacityDamagedProperty, value); }
		}
		#endregion

		#region Marines
		public static PropertyInfo<int> MarinesProperty = RegisterProperty<int>(c => c.Marines);
		public int Marines
		{
			get { return GetProperty(MarinesProperty); }
			private set { LoadProperty(MarinesProperty, value); }
		}

		public static PropertyInfo<int> MarinesDamagedProperty = RegisterProperty<int>(c => c.MarinesDamaged);
		public int MarinesDamaged
		{
			get { return GetProperty(MarinesDamagedProperty); }
			private set { LoadProperty(MarinesDamagedProperty, value); }
		}
		#endregion

		#endregion

		#region BattleShip Properties

		#region Rate
		public static PropertyInfo<RelativeRate> CurrentRateProperty = RegisterProperty<RelativeRate>(c => c.CurrentRate);
		public RelativeRate CurrentRate
		{
			get { return GetProperty(CurrentRateProperty); }
			set { SetProperty(CurrentRateProperty, value); }
		}
		#endregion

		#region Broadsides Fired
		public static PropertyInfo<bool> FiredPortProperty = RegisterProperty<bool>(c => c.FiredPort);
		public bool FiredPort
		{
			get { return GetProperty(FiredPortProperty); }
			set { SetProperty(FiredPortProperty, value); }
		}

		public static PropertyInfo<bool> FirstPortFiredProperty = RegisterProperty<bool>(c => c.FirstPortFired);
		public bool FirstPortFired
		{
			get { return GetProperty(FirstPortFiredProperty); }
			set { SetProperty(FirstPortFiredProperty, value); }
		}

		public static PropertyInfo<bool> FiredStarboardProperty = RegisterProperty<bool>(c => c.FiredStarboard);
		public bool FiredStarboard
		{
			get { return GetProperty(FiredStarboardProperty); }
			set { SetProperty(FiredStarboardProperty, value); }
		}

		public static PropertyInfo<bool> FirstStarboardFiredProperty = RegisterProperty<bool>(c => c.FirstStarboardFired);
		public bool FirstStarboardFired
		{
			get { return GetProperty(FirstStarboardFiredProperty); }
			set { SetProperty(FirstStarboardFiredProperty, value); }
		}
		#endregion

		#region Hits Taken

		#region Hull
		public static PropertyInfo<int> HullHitsProperty = RegisterProperty<int>(c => c.HullHits);
		public int HullHits
		{
			get { return GetProperty(HullHitsProperty); }
			set { SetProperty(HullHitsProperty, value); }
		}

		public static PropertyInfo<bool> DamagedProperty = RegisterProperty<bool>(c => c.Damaged);
		public bool Damaged
		{
			get { return GetProperty(DamagedProperty); }
			private set { LoadProperty(DamagedProperty, value); }
		}

		public static PropertyInfo<bool> VunerableProperty = RegisterProperty<bool>(c => c.Vunerable);
		public bool Vunerable
		{
			get { return GetProperty(VunerableProperty); }
			private set { LoadProperty(VunerableProperty, value); }
		}

		public int TotalDamageCapacity
		{
			get { return DamageCapacity + DamageCapacityDamaged; }
		}

		private class DamagedRule : BusinessRule
		{
			public DamagedRule()
				: base(HullHitsProperty)
			{
				AffectedProperties.Add(DamagedProperty);
				AffectedProperties.Add(CurrentRateProperty);
				AffectedProperties.Add(VunerableProperty);
			}

			protected override void Execute(RuleContext context)
			{
				var ship = (BattleShip)context.Target;
				if (ship.HullHits >= ship.DamageCapacity)
				{
					context.AddOutValue(DamagedProperty, true);
					context.AddOutValue(CurrentRateProperty, ship.RateDamaged);
				}
				else
				{
					context.AddOutValue(DamagedProperty, false);
					context.AddOutValue(CurrentRateProperty, ship.Rate);
				}
				if ((ship.TotalDamageCapacity - ship.HullHits) > 3)
				{
					context.AddOutValue(VunerableProperty, false);
				}
				else
				{
					context.AddOutValue(VunerableProperty, true);
				}
			}
		}
		#endregion

		#region Rigging
		public static PropertyInfo<int> RiggingHitsProperty = RegisterProperty<int>(c => c.RiggingHits);
		public int RiggingHits
		{
			get { return GetProperty(RiggingHitsProperty); }
			set { SetProperty(RiggingHitsProperty, value); }
		}

		public static PropertyInfo<bool> DismastedProperty = RegisterProperty<bool>(c => c.Dismasted);
		public bool Dismasted
		{
			get { return GetProperty(DismastedProperty); }
			private set { LoadProperty(DismastedProperty, value); }
		}

		private class DismastedRule : BusinessRule
		{
			public DismastedRule()
				: base(RiggingHitsProperty)
			{
				AffectedProperties.Add(DismastedProperty);
			}

			protected override void Execute(RuleContext context)
			{
				var ship = (BattleShip)context.Target;
				if (ship.RiggingHits >= 15)
				{
					context.AddOutValue(DismastedProperty, true);
				}
				else
				{
					context.AddOutValue(DismastedProperty, false);
				}
			}
		}
		#endregion

		#region Marines
		public static PropertyInfo<int> MarineHitsProperty = RegisterProperty<int>(c => c.MarineHits);
		public int MarineHits
		{
			get { return GetProperty(MarineHitsProperty); }
			set { SetProperty(MarineHitsProperty, value); }
		}
		#endregion

		#endregion

		#region Command Determination
		public static PropertyInfo<bool> OutOfCommandProperty = RegisterProperty<bool>(c => c.OutOfCommand);
		public bool OutOfCommand
		{
			get { return GetProperty(OutOfCommandProperty); }
			set { SetProperty(OutOfCommandProperty, value); }
		}

		public static PropertyInfo<int> CommandGroupProperty = RegisterProperty<int>(c => c.CommandGroup);
		public int CommandGroup
		{
			get { return GetProperty(CommandGroupProperty); }
			set { SetProperty(CommandGroupProperty, value); }
		}
		#endregion

		#region Status
		public static PropertyInfo<bool> InIronsProperty = RegisterProperty<bool>(c => c.InIrons);
		public bool InIrons
		{
			get { return GetProperty(InIronsProperty); }
			set { SetProperty(InIronsProperty, value); }
		}

		public static PropertyInfo<bool> AdriftProperty = RegisterProperty<bool>(c => c.Adrift);
		public bool Adrift
		{
			get { return GetProperty(AdriftProperty); }
			set { SetProperty(AdriftProperty, value); }
		}

		public static PropertyInfo<bool> AnchoredProperty = RegisterProperty<bool>(c => c.Anchored);
		public bool Anchored
		{
			get { return GetProperty(AnchoredProperty); }
			set { SetProperty(AnchoredProperty, value); }
		}

		public static PropertyInfo<bool> FullSailsProperty = RegisterProperty<bool>(c => c.FullSails);
		public bool FullSails
		{
			get { return GetProperty(FullSailsProperty); }
			set { SetProperty(FullSailsProperty, value); }
		}

		public static PropertyInfo<bool> AgroundProperty = RegisterProperty<bool>(c => c.Aground);
		public bool Aground
		{
			get { return GetProperty(AgroundProperty); }
			set { SetProperty(AgroundProperty, value); }
		}

		public static PropertyInfo<bool> OnFireProperty = RegisterProperty<bool>(c => c.OnFire);
		public bool OnFire
		{
			get { return GetProperty(OnFireProperty); }
			set { SetProperty(OnFireProperty, value); }
		}

		public static PropertyInfo<bool> StruckProperty = RegisterProperty<bool>(c => c.Struck);
		public bool Struck
		{
			get { return GetProperty(StruckProperty); }
			set { SetProperty(StruckProperty, value); }
		}

		public static PropertyInfo<bool> CapturedProperty = RegisterProperty<bool>(c => c.Captured);
		public bool Captured
		{
			get { return GetProperty(CapturedProperty); }
			set { SetProperty(CapturedProperty, value); }
		}
		#endregion

		#region Activation
		public static PropertyInfo<bool> MovedProperty = RegisterProperty<bool>(c => c.Moved);
		public bool Moved
		{
			get { return GetProperty(MovedProperty); }
			set { SetProperty(MovedProperty, value); }
		}
		#endregion

		#endregion

		#region Business Rules

		protected override void AddBusinessRules()
		{
			base.AddBusinessRules();
			BusinessRules.AddRule(new DismastedRule());
			BusinessRules.AddRule(new DamagedRule());
		}

		#endregion
	}
}
