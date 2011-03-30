using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Csla;
using Csla.Core;
using Csla.Rules;
using FlyingColors.DataModel;

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

		public static PropertyInfo<int> YearProperty = RegisterProperty<int>(c => c.Year);
		public int Year
		{
			get { return GetProperty(YearProperty); }
			private set { LoadProperty(YearProperty, value); }
		}

		public static PropertyInfo<Weather> WeatherProperty = RegisterProperty<Weather>(c => c.Weather);
		public Weather Weather
		{
			get { return GetProperty(WeatherProperty); }
			private set { LoadProperty(WeatherProperty, value); }
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

		public static readonly PropertyInfo<List<byte>> FlagProperty = RegisterProperty<List<byte>>(c => c.Flag);
		public List<byte> Flag
		{
			get { return GetProperty(FlagProperty); }
			private set { LoadProperty(FlagProperty, value); }
		}

		private class SetFlagRule : BusinessRule
		{
			public SetFlagRule()
				: base(NationalityProperty)
			{
				AffectedProperties.Add(FlagProperty);
			}

			protected override void Execute(RuleContext context)
			{
				var battleShip = (BattleShip)context.Target;
				List<Byte> flag = new List<Byte>();
				try
				{
					flag.AddRange(battleShip.GetFlag());
				}
				catch
				{
					context.AddErrorResult("Failed to load national flag.");
				}
				if (flag != null)
				{
					context.AddOutValue(FlagProperty, flag);
					context.AddSuccessResult(false);
				}
			}
		}

		private byte[] GetFlag()
		{
			switch (Nationality)
			{
				case FlyingColors.Nationality.British:
					return ConvertBitmapToByteArray(Flags.Resources.gb);
				case FlyingColors.Nationality.French:
					return ConvertBitmapToByteArray(Flags.Resources.fr);
				case FlyingColors.Nationality.Spanish:
					return ConvertBitmapToByteArray(Flags.Resources.es);
				case FlyingColors.Nationality.Danish:
					return ConvertBitmapToByteArray(Flags.Resources.dk);
				case FlyingColors.Nationality.Dutch:
					return ConvertBitmapToByteArray(Flags.Resources.nl);
				case FlyingColors.Nationality.American:
					return ConvertBitmapToByteArray(Flags.Resources.us);
				default:
					return ConvertBitmapToByteArray(Flags.Resources.gb);
			}
		}

		private byte[] ConvertBitmapToByteArray(Bitmap bmp)
		{
			byte[] img = null;
			using (MemoryStream ms = new MemoryStream())
			{
				bmp.Save(ms, ImageFormat.Png);
				img = ms.ToArray();
			}
			return img;
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

		public bool IsSmallVessel
		{
			get
			{
				return (Rate.Symbol == RelativeRateSymbol.Gunboat || Rate.Symbol == RelativeRateSymbol.GunBrig);
			}
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
		/// <summary>
		/// The current base Rate of the ship; equal to its Undamaged or Damaged Relative Rate.
		/// </summary>
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

		internal void ApplyDamage(Damage damage)
		{
			HullHits += damage.Hull;
			RiggingHits += damage.Rigging;
			MarineHits += damage.Marines;
		}

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

		private class HullHitsRule : BusinessRule
		{
			public HullHitsRule()
				: base(HullHitsProperty)
			{
				AffectedProperties.Add(DamagedProperty);
				AffectedProperties.Add(CurrentRateProperty);
				AffectedProperties.Add(VunerableProperty);
				AffectedProperties.Add(HullHitRateShiftsProperty);
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
				int rem = 0;
				int hullHitRateShift = Math.DivRem(ship.HullHits, 6, out rem);
				context.AddOutValue(HullHitRateShiftsProperty, 0 - hullHitRateShift);
			}
		}

		public static PropertyInfo<int> HullHitRateShiftsProperty = RegisterProperty<int>(c => c.HullHitRateShifts);
		public int HullHitRateShifts
		{
			get { return GetProperty(HullHitRateShiftsProperty); }
			private set { LoadProperty(HullHitRateShiftsProperty, value); }
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

		public static PropertyInfo<int> MovementPenaltyProperty = RegisterProperty<int>(c => c.MovementPenalty);
		public int MovementPenalty
		{
			get { return GetProperty(MovementPenaltyProperty); }
			private set { LoadProperty(MovementPenaltyProperty, value); }
		}

		private class RiggingHitsRule : BusinessRule
		{
			public RiggingHitsRule()
				: base(RiggingHitsProperty)
			{
				AffectedProperties.Add(DismastedProperty);
				AffectedProperties.Add(AdriftProperty);
				AffectedProperties.Add(CanFullSailsProperty);
				AffectedProperties.Add(FullSailsProperty);
				AffectedProperties.Add(MovementPenaltyProperty);
			}

			protected override void Execute(RuleContext context)
			{
				var ship = (BattleShip)context.Target;
				if (ship.RiggingHits >= 15)
				{
					context.AddOutValue(DismastedProperty, true);
					context.AddOutValue(AdriftProperty, true);
				}
				else
				{
					context.AddOutValue(DismastedProperty, false);
				}
				if (ship.RiggingHits >= 6)
				{
					context.AddOutValue(CanFullSailsProperty, false);
					context.AddOutValue(FullSailsProperty, false);
				}
				int remainder = 0;
				context.AddOutValue(MovementPenaltyProperty,
					Math.DivRem(ship.RiggingHits, 3, out remainder));
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

		public static readonly PropertyInfo<int> MarinesRemainingProperty = RegisterProperty<int>(c => c.MarinesRemaining);
		public int MarinesRemaining
		{
			get { return GetProperty(MarinesRemainingProperty); }
			private set { LoadProperty(MarinesRemainingProperty, value); }
		}

		private class MarineHitsRule : BusinessRule
		{
			public MarineHitsRule()
				: base(MarineHitsProperty)
			{
				AffectedProperties.Add(MarinesRemainingProperty);
			}
			protected override void Execute(RuleContext context)
			{
				var ship = (BattleShip)context.Target;
				int marinesCount = ship.Marines - ship.MarineHits;
				marinesCount = marinesCount > 1 ? marinesCount : 1;
				context.AddOutValue(MarinesRemainingProperty, marinesCount);
			}
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

		private static readonly string InIronsString = "In Irons";

		private class InIronsRule : BusinessRule
		{
			public InIronsRule()
				: base(InIronsProperty)
			{
				AffectedProperties.Add(StatusProperty);
			}

			protected override void Execute(RuleContext context)
			{
				var ship = (BattleShip)context.Target;
				if (ship.InIrons)
				{

				}
				else
				{

				}
			}
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

		public static readonly PropertyInfo<bool> CanFullSailsProperty = RegisterProperty<bool>(c => c.CanFullSails);
		public bool CanFullSails
		{
			get { return GetProperty(CanFullSailsProperty); }
			private set { LoadProperty(CanFullSailsProperty, value); }
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

		#region Movement
		public static PropertyInfo<bool> MovedProperty = RegisterProperty<bool>(c => c.Moved);
		public bool Moved
		{
			get { return GetProperty(MovedProperty); }
			set { SetProperty(MovedProperty, value); }
		}

		#endregion

		#region Status Report
		public static PropertyInfo<string> StatusProperty = RegisterProperty<string>(c => c.Status);
		public string Status
		{
			get { return GetProperty(StatusProperty); }
			private set { LoadProperty(StatusProperty, value); }
		}

		private class UpdateStatusRule : BusinessRule
		{
			public UpdateStatusRule(IPropertyInfo primaryProperty)
				: base(primaryProperty)
			{
				AffectedProperties.Add(StatusProperty);
			}

			protected override void Execute(RuleContext context)
			{

			}
		}
		#endregion

		#endregion

		#region Business Rules

		protected override void AddBusinessRules()
		{
			base.AddBusinessRules();
			BusinessRules.AddRule(new RiggingHitsRule());
			BusinessRules.AddRule(new HullHitsRule());
			BusinessRules.AddRule(new MarineHitsRule());
			BusinessRules.AddRule(new SetFlagRule());
		}

		#endregion

		#region Object Overrides

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj is BattleShip)
			{
				var other = (BattleShip)obj;
				return this.BattleShipId == other.BattleShipId;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return BattleShipId.GetHashCode();
		}

		public override string ToString()
		{
			return Name;
		}

		#endregion

		#region Factory Methods
		internal static BattleShip NewBattleShip(FleetData fleet, FleetShipData ship)
		{
			return DataPortal.CreateChild<BattleShip>(new Tuple<FleetData, FleetShipData>(fleet, ship));
		}

		internal static BattleShip GetShip(BattleShipData ship)
		{
			return DataPortal.FetchChild<BattleShip>(ship);
		}
		#endregion

		#region Data Portal

		private BattleShipData _ship = null;

		[RunLocal]
		private void Child_Create(Tuple<FleetData, FleetShipData> battleShipTuple)
		{
			_ship = new BattleShipData();
			_ship.FleetShip = battleShipTuple.Item2;
			// Fleet
			LoadProperty<int>(AudacityProperty, battleShipTuple.Item1.Audacity);
			LoadProperty<int>(YearProperty, battleShipTuple.Item1.Scenario.Year);
			LoadProperty<Weather>(WeatherProperty, battleShipTuple.Item1.Scenario.WeatherEffects);
			// Commanders
			LoadProperty<BattleShipCommanderList>(CommandersProperty,
				BattleShipCommanderList.New(_ship, battleShipTuple.Item2.Commanders));

			// Ship
			LoadProperty<string>(NameProperty, battleShipTuple.Item2.Ship.Name);
			LoadProperty<Nationality>(NationalityProperty, (Nationality)Enum.Parse(typeof(Nationality), battleShipTuple.Item2.Ship.Nationality));
			LoadProperty<int>(VictoryPointsProperty, battleShipTuple.Item2.Ship.VictoryPoints);
			LoadProperty<RelativeRate>(RateProperty, RelativeRate.NewRate(
				_ship.FleetShip.Ship.Rate,
				(RelativeRateColor)Enum.Parse(typeof(RelativeRateColor), _ship.FleetShip.Ship.RateColor),
				(RelativeRateShape)Enum.Parse(typeof(RelativeRateShape), _ship.FleetShip.Ship.RateShape)));
			LoadProperty<RelativeRate>(RateDamagedProperty, RelativeRate.NewRate(
				_ship.FleetShip.Ship.RateDamaged,
				(RelativeRateColor)Enum.Parse(typeof(RelativeRateColor), _ship.FleetShip.Ship.RateColorDamaged),
				(RelativeRateShape)Enum.Parse(typeof(RelativeRateShape), _ship.FleetShip.Ship.RateShapeDamaged)));
			LoadProperty<int>(DamageCapacityProperty, battleShipTuple.Item2.Ship.DamageCapacity);
			LoadProperty<int>(DamageCapacityDamagedProperty, battleShipTuple.Item2.Ship.DamageCapacityDamaged);
			LoadProperty<int>(MarinesProperty, battleShipTuple.Item2.Ship.Marines);
			LoadProperty<int>(MarinesDamagedProperty, battleShipTuple.Item2.Ship.MarinesDamaged);

			// Battle Ship
			LoadProperty<RelativeRate>(CurrentRateProperty, Rate);
			// Fired
			LoadProperty<bool>(FirstPortFiredProperty, false);
			LoadProperty<bool>(FiredPortProperty, false);
			LoadProperty<bool>(FirstStarboardFiredProperty, false);
			LoadProperty<bool>(FiredStarboardProperty, false);
			// Hull Hits
			LoadProperty<int>(HullHitsProperty, battleShipTuple.Item2.HullHitsAtStart);
			LoadProperty<bool>(DamagedProperty, false);
			LoadProperty<bool>(VunerableProperty, false);
			// Rigging Hits
			LoadProperty<int>(RiggingHitsProperty, battleShipTuple.Item2.RiggingHitsAtStart);
			LoadProperty<bool>(DismastedProperty, false);
			LoadProperty<int>(MovementPenaltyProperty, 0);
			// Marine Hits
			LoadProperty<int>(MarineHitsProperty, 0);
			LoadProperty<int>(MarinesRemainingProperty, Marines);
			// Command
			LoadProperty<bool>(OutOfCommandProperty, false);
			LoadProperty<int>(CommandGroupProperty, 0);
			// Status
			LoadProperty<bool>(InIronsProperty, false);
			LoadProperty<bool>(AdriftProperty, false);
			LoadProperty<bool>(AnchoredProperty, false);
			LoadProperty<bool>(CanFullSailsProperty, true);
			LoadProperty<bool>(FullSailsProperty, false);
			LoadProperty<bool>(AgroundProperty, false);
			LoadProperty<bool>(OnFireProperty, false);
			LoadProperty<bool>(StruckProperty, false);
			LoadProperty<bool>(CapturedProperty, false);
			// Activation
			LoadProperty<bool>(MovedProperty, false);
			// Report
			LoadProperty<string>(StatusProperty, string.Empty);

			BusinessRules.CheckRules();
		}

		private void Child_Fetch(BattleShipData ship)
		{
			_ship = ship;
			// Fleet
			LoadProperty<int>(AudacityProperty, _ship.FleetShip.Fleet.Audacity);
			LoadProperty<int>(YearProperty, _ship.FleetShip.Fleet.Scenario.Year);
			LoadProperty<Weather>(WeatherProperty, _ship.FleetShip.Fleet.Scenario.WeatherEffects);

			// Commanders
			LoadProperty<BattleShipCommanderList>(CommandersProperty,
				BattleShipCommanderList.GetCommanders(_ship.Commanders));

			// Ship
			LoadProperty<string>(NameProperty, _ship.FleetShip.Ship.Name);
			LoadProperty<Nationality>(NationalityProperty, (Nationality)Enum.Parse(typeof(Nationality), _ship.FleetShip.Ship.Nationality));
			LoadProperty<int>(VictoryPointsProperty, _ship.FleetShip.Ship.VictoryPoints);
			LoadProperty<RelativeRate>(RateProperty, RelativeRate.NewRate(
				_ship.FleetShip.Ship.Rate,
				(RelativeRateColor)Enum.Parse(typeof(RelativeRateColor), _ship.FleetShip.Ship.RateColor),
				(RelativeRateShape)Enum.Parse(typeof(RelativeRateShape), _ship.FleetShip.Ship.RateShape)));
			LoadProperty<RelativeRate>(RateDamagedProperty, RelativeRate.NewRate(
				_ship.FleetShip.Ship.RateDamaged,
				(RelativeRateColor)Enum.Parse(typeof(RelativeRateColor), _ship.FleetShip.Ship.RateColorDamaged),
				(RelativeRateShape)Enum.Parse(typeof(RelativeRateShape), _ship.FleetShip.Ship.RateShapeDamaged)));
			LoadProperty<int>(DamageCapacityProperty, _ship.FleetShip.Ship.DamageCapacity);
			LoadProperty<int>(DamageCapacityDamagedProperty, _ship.FleetShip.Ship.DamageCapacityDamaged);
			LoadProperty<int>(MarinesProperty, _ship.FleetShip.Ship.Marines);
			LoadProperty<int>(MarinesDamagedProperty, _ship.FleetShip.Ship.MarinesDamaged);

			// Battle Ship
			LoadProperty<RelativeRate>(CurrentRateProperty, Rate);
			// Fired
			LoadProperty<bool>(FirstPortFiredProperty, _ship.FirstPortFired);
			LoadProperty<bool>(FiredPortProperty, _ship.FiredPort);
			LoadProperty<bool>(FirstStarboardFiredProperty, _ship.FirstStarboardFired);
			LoadProperty<bool>(FiredStarboardProperty, _ship.FiredStarboard);
			// Hull Hits
			LoadProperty<int>(HullHitsProperty, _ship.HullHits);
			LoadProperty<bool>(DamagedProperty, false);
			LoadProperty<bool>(VunerableProperty, false);
			// Rigging Hits
			LoadProperty<int>(RiggingHitsProperty, _ship.RiggingHits);
			LoadProperty<bool>(DismastedProperty, false);
			LoadProperty<int>(MovementPenaltyProperty, 0);
			// Marine Hits
			LoadProperty<int>(MarineHitsProperty, _ship.MarineHits);
			LoadProperty<int>(MarinesRemainingProperty, Marines - MarineHits);
			// Command
			LoadProperty<bool>(OutOfCommandProperty, _ship.OutOfCommand);
			LoadProperty<int>(CommandGroupProperty, _ship.CommandGroup);
			// Status
			LoadProperty<bool>(InIronsProperty, _ship.InIrons);
			LoadProperty<bool>(AdriftProperty, _ship.Adrift);
			LoadProperty<bool>(AnchoredProperty, _ship.Anchored);
			LoadProperty<bool>(CanFullSailsProperty, true);
			LoadProperty<bool>(FullSailsProperty, _ship.FullSails);
			LoadProperty<bool>(AgroundProperty, _ship.Aground);
			LoadProperty<bool>(OnFireProperty, _ship.OnFire);
			LoadProperty<bool>(StruckProperty, _ship.Struck);
			LoadProperty<bool>(CapturedProperty, _ship.Captured);
			// Activation
			LoadProperty<bool>(MovedProperty, _ship.Moved);
			// Report
			LoadProperty<string>(StatusProperty, string.Empty);

			BusinessRules.CheckRules();
		}

		private void Child_Insert(BattleGroupData battleGroup)
		{
			LoadProperty<long>(BattleShipIdProperty, _ship.BattleShipId);
			FieldManager.UpdateChildren(_ship);
		}

		private void Child_Update(BattleGroupData battleGroup)
		{
			FieldManager.UpdateChildren(_ship);
		}

		private void Child_Delete(BattleGroupData battleGroup)
		{
			FieldManager.UpdateChildren(_ship);
		}

		internal BattleShipData ToData(BattleGroupData battleGroup)
		{
			_ship.BattleGroup = battleGroup;
			_ship.Adrift = ReadProperty<bool>(AdriftProperty);
			_ship.Aground = ReadProperty<bool>(AgroundProperty);
			_ship.Anchored = ReadProperty<bool>(AnchoredProperty);
			_ship.Captured = ReadProperty<bool>(CapturedProperty);
			_ship.CommandGroup = ReadProperty<int>(CommandGroupProperty);
			_ship.FiredPort = ReadProperty<bool>(FiredPortProperty);
			_ship.FirstPortFired = ReadProperty<bool>(FirstPortFiredProperty);
			_ship.FirstStarboardFired = ReadProperty<bool>(FirstStarboardFiredProperty);
			_ship.FullSails = ReadProperty<bool>(FullSailsProperty);
			_ship.HullHits = ReadProperty<int>(HullHitsProperty);
			_ship.InIrons = ReadProperty<bool>(InIronsProperty);
			_ship.MarineHits = ReadProperty<int>(MarineHitsProperty);
			_ship.Moved = ReadProperty<bool>(MovedProperty);
			_ship.OnFire = ReadProperty<bool>(OnFireProperty);
			_ship.OutOfCommand = ReadProperty<bool>(OutOfCommandProperty);
			_ship.RiggingHits = ReadProperty<int>(RiggingHitsProperty);
			_ship.Struck = ReadProperty<bool>(StruckProperty);
			_ship.Commanders = ReadProperty<BattleShipCommanderList>(CommandersProperty).ToData(_ship);
			return _ship;
		}

		//[Flags]
		//private enum BattleShipStates
		//{
		//    [Description("")]
		//    None = 0,
		//    [Description("Fired Port")]
		//    FiredPort = 1,
		//    [Description("Fired Starboard")]
		//    FiredStarboard = 2,
		//    [Description("Fired Both")]
		//    FiredBoth = FiredPort | FiredStarboard,
		//    [Description("Adrift")]
		//    Adrift = 4,
		//    Aground = 8,
		//    FullSails = 16,
		//    Anchored = 32,
		//    InIrons = 64,
		//    OnFire = 128,
		//    Struck = 256,
		//    OutOfCommand = 512,
		//    Dismasted = 1024,
		//    Captured = 2048,
		//    Sunk = 4096,
		//    Moved = 8192
		//    
		//}

		//static class ExtensionMethods
		//{
		//    public static string ToDescription(this Enum en) //ext method
		//    {
		//        Type type = en.GetType();
		//        MemberInfo[] memInfo = type.GetMember(en.ToString());
		//        if (memInfo != null && memInfo.Length > 0)
		//        {
		//            object[] attrs =
		//                memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
		//            if (attrs != null && attrs.Length > 0)
		//            {
		//                return ((DescriptionAttribute)attrs[0]).Description;
		//            }
		//        }
		//        return en.ToString();

		//    }
		//}

		#endregion




	}
}
