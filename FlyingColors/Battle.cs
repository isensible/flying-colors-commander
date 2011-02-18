using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;

namespace FlyingColors
{
	[Serializable]
	public class Battle : BusinessBase<Battle>
	{
		#region Battle Properties
		public static PropertyInfo<long> BattleIdProperty = RegisterProperty<long>(c => c.BattleId);
		public long BattleId
		{
			get { return GetProperty(BattleIdProperty); }
			set { SetProperty(BattleIdProperty, value); }
		}

		public static PropertyInfo<string> SummaryProperty = RegisterProperty<string>(c => c.Summary);
		public string Summary
		{
			get { return GetProperty(SummaryProperty); }
			set { SetProperty(SummaryProperty, value); }
		}

		public static PropertyInfo<SmartDate> StartedProperty = RegisterProperty<SmartDate>(c => c.Started, "Name", new SmartDate());
		public string Started
		{
			get { return GetPropertyConvert<SmartDate, string>(StartedProperty); }
			set { SetPropertyConvert<SmartDate, string>(StartedProperty, value); }
		}

		public static PropertyInfo<SmartDate> FinishedProperty = RegisterProperty<SmartDate>(c => c.Finished, "Name", new SmartDate());
		public string Finished
		{
			get { return GetPropertyConvert<SmartDate, string>(FinishedProperty); }
			set { SetPropertyConvert<SmartDate, string>(FinishedProperty, value); }
		}

		public static PropertyInfo<int> TurnProperty = RegisterProperty<int>(c => c.Turn);
		public int Turn
		{
			get { return GetProperty(TurnProperty); }
			set { SetProperty(TurnProperty, value); }
		}

		public static PropertyInfo<Weather> WeatherProperty = RegisterProperty<Weather>(c => c.Weather);
		public Weather Weather
		{
			get { return GetProperty(WeatherProperty); }
			set { SetProperty(WeatherProperty, value); }
		}

		public static PropertyInfo<int> WindDirectionProperty = RegisterProperty<int>(c => c.WindDirection);
		public int WindDirection
		{
			get { return GetProperty(WindDirectionProperty); }
			set { SetProperty(WindDirectionProperty, value); }
		}

		public static PropertyInfo<Nationality> VictorProperty = RegisterProperty<Nationality>(c => c.Victor);
		public Nationality Victor
		{
			get { return GetProperty(VictorProperty); }
			set { SetProperty(VictorProperty, value); }
		}
		#endregion

		#region Scenario
		public static PropertyInfo<long> ScenarioIdProperty = RegisterProperty<long>(c => c.ScenarioId);
		public long ScenarioId
		{
			get { return GetProperty(ScenarioIdProperty); }
			private set { LoadProperty(ScenarioIdProperty, value); }
		}

		public static PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
		public string Name
		{
			get { return GetProperty(NameProperty); }
			private set { LoadProperty(NameProperty, value); }
		}

		public static PropertyInfo<int> YearProperty = RegisterProperty<int>(c => c.Year);
		public int Year
		{
			get { return GetProperty(YearProperty); }
			private set { LoadProperty(YearProperty, value); }
		}

		public static PropertyInfo<int> TurnsProperty = RegisterProperty<int>(c => c.Turns);
		public int Turns
		{
			get { return GetProperty(TurnsProperty); }
			private set { LoadProperty(TurnsProperty, value); }
		}

		public static PropertyInfo<string> MapsProperty = RegisterProperty<string>(c => c.Maps);
		public string Maps
		{
			get { return GetProperty(MapsProperty); }
			private set { LoadProperty(MapsProperty, value); }
		}
		#endregion

		#region Battle Groups

		public static PropertyInfo<BattleGroup> BattleGroupAProperty = RegisterProperty<BattleGroup>(c => c.BattleGroupA);
		public BattleGroup BattleGroupA
		{
			get { return GetProperty(BattleGroupAProperty); }
			set { SetProperty(BattleGroupAProperty, value); }
		}

		public static PropertyInfo<BattleGroup> BattleGroupBProperty = RegisterProperty<BattleGroup>(c => c.BattleGroupB);
		public BattleGroup BattleGroupB
		{
			get { return GetProperty(BattleGroupBProperty); }
			set { SetProperty(BattleGroupBProperty, value); }
		}

		#endregion

		#region Factory Methods

		public static Battle NewBattle(Scenario scenario)
		{
			return DataPortal.Create<Battle>(scenario);
		}

		#endregion

		#region Data Portal

		[RunLocal]
		private void DataPortal_Create(Scenario scenario)
		{
			// Battle properties
			LoadProperty<string>(SummaryProperty, scenario.Name);
			LoadProperty<SmartDate>(StartedProperty, DateTime.Now);
			LoadProperty<SmartDate>(FinishedProperty, DateTime.MaxValue);
			LoadProperty<int>(TurnProperty, 1);
			LoadProperty<Weather>(WeatherProperty, scenario.WeatherEffects);
			LoadProperty<int>(WindDirectionProperty, scenario.WindDirection);
			LoadProperty<Nationality>(VictorProperty, Nationality.Unknown);
			// Scenario properties
			LoadProperty<long>(ScenarioIdProperty, scenario.ScenarioId);
			LoadProperty<string>(NameProperty, scenario.Name);
			LoadProperty<int>(YearProperty, scenario.Year);
			LoadProperty<string>(MapsProperty, scenario.Maps);
			LoadProperty<int>(TurnsProperty, scenario.Turns);
			// Create the two battle groups
			var battleGroups = from fleet in scenario.Fleets
							   group fleet by fleet.BattleGroup into g
							   select new { Nationality = g.Key, Fleets = g };
			foreach (var group in battleGroups)
			{
				if (BattleGroupA == null)
				{
					LoadProperty<BattleGroup>(BattleGroupAProperty, BattleGroup.NewBattleGroup(group.Nationality));
					foreach (var fleet in group.Fleets)
					{
						BattleGroupA.AddFleet(fleet);
					}
					continue;
				}
				if (BattleGroupB == null)
				{
					LoadProperty<BattleGroup>(BattleGroupAProperty, BattleGroup.NewBattleGroup(group.Nationality));
					foreach (var fleet in group.Fleets)
					{
						BattleGroupA.AddFleet(fleet);
					}
					continue;
				}
			}
			//LoadProperty<BattleGroup>(BattleGroupAProperty, BattleShipList.New());
			//LoadProperty<BattleGroup>(BattleGroupBProperty, BattleShipList.New());
		}

		#endregion
	}
}
