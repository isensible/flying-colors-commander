using System;
using Castle.ActiveRecord;
using Csla;
using FlyingColors.DataModel;

namespace FlyingColors
{
	[Serializable]
	public class Scenario : BusinessBase<Scenario>
	{
		private ScenarioData _scenario = null;

		public static readonly PropertyInfo<long> ScenarioIdProperty = RegisterProperty<long>(c => c.ScenarioId);
		/// <Summary>
		/// Gets or sets the ScenarioId value.
		/// </Summary>
		public long ScenarioId
		{
			get { return GetProperty(ScenarioIdProperty); }
			set { SetProperty(ScenarioIdProperty, value); }
		}

		public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
		/// <Summary>
		/// Gets or sets the Name value.
		/// </Summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}

		public static readonly PropertyInfo<int> YearProperty = RegisterProperty<int>(c => c.Year);
		/// <Summary>
		/// Gets or sets the Year value.
		/// </Summary>
		public int Year
		{
			get { return GetProperty(YearProperty); }
			set { SetProperty(YearProperty, value); }
		}

		public static readonly PropertyInfo<int> WindDirectionProperty = RegisterProperty<int>(c => c.WindDirection);
		/// <Summary>
		/// Gets or sets the WindDirection value.
		/// </Summary>
		public int WindDirection
		{
			get { return GetProperty(WindDirectionProperty); }
			set { SetProperty(WindDirectionProperty, value); }
		}

		public static readonly PropertyInfo<Weather> WeatherEffectsProperty = RegisterProperty<Weather>(c => c.WeatherEffects);
		/// <Summary>
		/// Gets or sets the WeatherEffects value.
		/// </Summary>
		public Weather WeatherEffects
		{
			get { return GetProperty(WeatherEffectsProperty); }
			set { SetProperty(WeatherEffectsProperty, value); }
		}

		public static readonly PropertyInfo<string> MapsProperty = RegisterProperty<string>(c => c.Maps);
		/// <Summary>
		/// Gets or sets the Maps value.
		/// </Summary>
		public string Maps
		{
			get { return GetProperty(MapsProperty); }
			set { SetProperty(MapsProperty, value); }
		}

		public static readonly PropertyInfo<int> TurnsProperty = RegisterProperty<int>(c => c.Turns);
		/// <Summary>
		/// Gets or sets the Turns value.
		/// </Summary>
		public int Turns
		{
			get { return GetProperty(TurnsProperty); }
			set { SetProperty(TurnsProperty, value); }
		}

		public static readonly PropertyInfo<FleetList> FleetsProperty = RegisterProperty<FleetList>(c => c.Fleets);
		/// <Summary>
		/// Gets or sets the Fleets value.
		/// </Summary>
		public FleetList Fleets
		{
			get { return GetProperty(FleetsProperty); }
			set { SetProperty(FleetsProperty, value); }
		}

		public static Scenario NewScenario()
		{
			return DataPortal.Create<Scenario>();
		}

		internal static Scenario GetScenario(ScenarioData scenario)
		{
			return DataPortal.Fetch<Scenario>(scenario);
		}

		protected override void DataPortal_Create()
		{
			_scenario = new ScenarioData();
			LoadProperty<string>(NameProperty, string.Empty);
			LoadProperty<int>(YearProperty, 1800);
			LoadProperty<int>(WindDirectionProperty, 1);
			LoadProperty<Weather>(WeatherEffectsProperty, Weather.Normal);
			LoadProperty<string>(MapsProperty, "A");
			LoadProperty<int>(TurnsProperty, 15);
			LoadProperty<FleetList>(FleetsProperty, FleetList.NewFleetList(_scenario.Fleets));
		}

		private void DataPortal_Fetch(ScenarioData scenarioData)
		{
			_scenario = scenarioData;
			LoadProperty<long>(ScenarioIdProperty, _scenario.ScenarioId);
			LoadProperty<string>(NameProperty, _scenario.Name);
			LoadProperty<int>(YearProperty, _scenario.Year);
			LoadProperty<int>(WindDirectionProperty, _scenario.WindDirection);
			LoadProperty<Weather>(WeatherEffectsProperty, _scenario.WeatherEffects);
			LoadProperty<string>(MapsProperty, _scenario.Maps);
			LoadProperty<int>(TurnsProperty, _scenario.Turns);
			LoadProperty<FleetList>(FleetsProperty, FleetList.GetFleetList(_scenario.Fleets));
		}

		protected override void DataPortal_Insert()
		{
			ToData();
			ActiveRecordMediator<ScenarioData>.Create(_scenario);
			LoadProperty<long>(ScenarioIdProperty, _scenario.ScenarioId);
		}

		protected override void DataPortal_Update()
		{
			ToData();
			ActiveRecordMediator<ScenarioData>.Update(_scenario);
		}

		protected override void DataPortal_DeleteSelf()
		{
			ActiveRecordMediator<ScenarioData>.Delete(ToData());
		}

		internal ScenarioData ToData()
		{
			_scenario.Maps = this.Maps;
			_scenario.Name = this.Name;
			_scenario.Turns = this.Turns;
			_scenario.WeatherEffects = WeatherEffects;
			_scenario.WindDirection = WindDirection;
			_scenario.Year = Year;
			_scenario.Fleets = Fleets.ToData(_scenario);
			return _scenario;
		}



	}
}
