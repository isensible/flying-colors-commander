using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using Csla.Rules;
using Csla.Core;
using FlyingColors.DataModel;
using Castle.ActiveRecord;

namespace FlyingColors
{
	[Serializable]
	public class BattleShipCommander : BusinessBase<BattleShipCommander>
	{
		public static PropertyInfo<long> BattleShipCommanderIdProperty = RegisterProperty<long>(c => c.BattleShipCommanderId);
		public long BattleShipCommanderId
		{
			get { return GetProperty(BattleShipCommanderIdProperty); }
			private set { LoadProperty(BattleShipCommanderIdProperty, value); }
		}

		public static PropertyInfo<bool> WoundedProperty = RegisterProperty<bool>(c => c.Wounded);
		public bool Wounded
		{
			get { return GetProperty(WoundedProperty); }
			set { SetProperty(WoundedProperty, value); }
		}

		public static PropertyInfo<bool> KilledProperty = RegisterProperty<bool>(c => c.Killed);
		public bool Killed
		{
			get { return GetProperty(KilledProperty); }
			set { SetProperty(KilledProperty, value); }
		}

		public static PropertyInfo<bool> CapturedProperty = RegisterProperty<bool>(c => c.Captured);
		public bool Captured
		{
			get { return GetProperty(CapturedProperty); }
			set { SetProperty(CapturedProperty, value); }
		}

		#region Current Quality and Range
		public static PropertyInfo<int> CurrentQualityProperty = RegisterProperty<int>(c => c.CurrentQuality);
		public int CurrentQuality
		{
			get { return GetProperty(CurrentQualityProperty); }
			private set { LoadProperty(CurrentQualityProperty, value); }
		}
		
		public static PropertyInfo<int> CurrentRangeProperty = RegisterProperty<int>(c => c.CurrentRange);
		public int CurrentRange
		{
			get { return GetProperty(CurrentRangeProperty); }
			private set { LoadProperty(CurrentRangeProperty, value); }
		}

		private class WoundedKilledOrCapturedRule : BusinessRule
		{
			public WoundedKilledOrCapturedRule(IPropertyInfo primaryProperty) 
				: base(primaryProperty)
			{
				AffectedProperties.Add(CurrentQualityProperty);
				AffectedProperties.Add(CurrentRangeProperty);
			}

			protected override void Execute(RuleContext context)
			{
				var commander = (BattleShipCommander)context.Target;
				if (commander.Killed || commander.Captured)
				{
					context.AddOutValue(CurrentQualityProperty, 0);
					context.AddOutValue(CurrentRangeProperty, 0);
				}
				else if (commander.Wounded)
				{
					context.AddOutValue(CurrentQualityProperty, commander.QualityWounded);
					context.AddOutValue(CurrentRangeProperty, commander.RangeWounded);
				}
				else
				{
					context.AddOutValue(CurrentQualityProperty, commander.Quality);
					context.AddOutValue(CurrentRangeProperty, commander.Range);
				}
			}
		}
		#endregion

		#region Commander Properties
		public static PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
		public string Name
		{
			get { return GetProperty(NameProperty); }
			private set { LoadProperty(NameProperty, value); }
		}

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

		public static PropertyInfo<int> RankProperty = RegisterProperty<int>(c => c.Rank);
		public int Rank
		{
			get { return GetProperty(RankProperty); }
			private set { LoadProperty(RankProperty, value); }
		}

		public static PropertyInfo<int> QualityProperty = RegisterProperty<int>(c => c.Quality);
		public int Quality
		{
			get { return GetProperty(QualityProperty); }
			private set { LoadProperty(QualityProperty, value); }
		}

		public static PropertyInfo<int> QualityWoundedProperty = RegisterProperty<int>(c => c.QualityWounded);
		public int QualityWounded
		{
			get { return GetProperty(QualityWoundedProperty); }
			private set { LoadProperty(QualityWoundedProperty, value); }
		}

		public static PropertyInfo<int> RangeProperty = RegisterProperty<int>(c => c.Range);
		public int Range
		{
			get { return GetProperty(RangeProperty); }
			private set { LoadProperty(RangeProperty, value); }
		}

		public static PropertyInfo<int> RangeWoundedProperty = RegisterProperty<int>(c => c.RangeWounded);
		public int RangeWounded
		{
			get { return GetProperty(RangeWoundedProperty); }
			private set { LoadProperty(RangeWoundedProperty, value); }
		}
		#endregion

		#region Business Rules

		protected override void AddBusinessRules()
		{
			base.AddBusinessRules();
			BusinessRules.AddRule(new WoundedKilledOrCapturedRule(WoundedProperty));
			BusinessRules.AddRule(new WoundedKilledOrCapturedRule(KilledProperty));
			BusinessRules.AddRule(new WoundedKilledOrCapturedRule(CapturedProperty));
		}

		#endregion

		#region Factory Methods
		internal static BattleShipCommander NewCommander(FleetShipCommanderData commander)
		{
			return DataPortal.CreateChild<BattleShipCommander>(commander);
		}

		private BattleShipCommander()
		{
		}
		#endregion

		#region Data Portal

		private BattleShipCommanderData _commander = null;

		private void Child_Create(FleetShipCommanderData commander)
		{
			_commander = new BattleShipCommanderData();
			_commander.Commander = commander.Commander;
			
			LoadProperty<bool>(WoundedProperty, false);
			LoadProperty<bool>(KilledProperty, false);
			LoadProperty<bool>(CapturedProperty, false);
			LoadProperty<int>(QualityProperty, commander.Commander.Quality);
			LoadProperty<int>(QualityWoundedProperty, commander.Commander.QualityWounded);
			LoadProperty<int>(CurrentQualityProperty, Quality);
			LoadProperty<int>(RangeProperty, commander.Commander.Range);
			LoadProperty<int>(RangeWoundedProperty, commander.Commander.RangeWounded);
			LoadProperty<int>(CurrentRangeProperty, Range);
			LoadProperty<string>(NameProperty, commander.Commander.Name);
			LoadProperty<Nationality>(NationalityProperty, (Nationality)Enum.Parse(typeof(Nationality), commander.Commander.Nationality));
			LoadProperty<int>(VictoryPointsProperty, commander.Commander.VictoryPoints);
			LoadProperty<int>(RankProperty, commander.Commander.Rank);
		}

		private void Child_Insert(BattleShipData ship)
		{
			ToData(ship);
			ActiveRecordMediator<BattleShipCommanderData>.Create(_commander);
		}

		private void Child_Update(BattleShipData ship)
		{
			ToData(ship);
			ActiveRecordMediator<BattleShipCommanderData>.Update(_commander);
		}

		private void Child_Delete(BattleShipData ship)
		{
			ToData(ship);
			ActiveRecordMediator<BattleShipCommanderData>.Delete(_commander);
		}

		internal BattleShipCommanderData ToData(BattleShipData ship)
		{
			_commander.Ship = ship;
			_commander.Captured = ReadProperty<bool>(CapturedProperty);
			_commander.Killed = ReadProperty<bool>(KilledProperty);
			_commander.Wounded = ReadProperty<bool>(WoundedProperty);
			return _commander;
		}

		#endregion
	}
}
