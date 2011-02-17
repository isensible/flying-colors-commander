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


	}
}
