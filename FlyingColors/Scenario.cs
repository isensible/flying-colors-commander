using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;

namespace FlyingColors
{
	[Serializable]
	public class Scenario : BusinessBase<Scenario>
	{
		public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
		/// <Summary>
		/// Gets or sets the Name value.
		/// </Summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}

		/// <summary>
		/// Fluent interface for setting Name property.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Scenario SetName(string name)
		{
			this.Name = name;
			return this;
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

		/// <summary>
		/// Fluent interface for setting Year property.
		/// </summary>
		/// <param name="year"></param>
		/// <returns></returns>
		public Scenario SetYear(int year)
		{
			this.Year = year;
			return this;
		}


		public static readonly PropertyInfo<FleetCollection> FleetsProperty = RegisterProperty<FleetCollection>(c => c.Fleets);
		/// <Summary>
		/// Gets or sets the Fleets value.
		/// </Summary>
		public FleetCollection Fleets
		{
			get { return GetProperty(FleetsProperty); }
			set { SetProperty(FleetsProperty, value); }
		}

		public static Scenario NewScenario()
		{
			return DataPortal.CreateChild<Scenario>();
		}

		internal static Scenario GetScenario(ScenarioData scenarioData)
		{
			return DataPortal.FetchChild<Scenario>(scenarioData);
		}

		private void Child_Create()
		{
			LoadProperty<string>(NameProperty, string.Empty);
			LoadProperty<int>(YearProperty, 1800);
			LoadProperty<FleetCollection>(FleetsProperty, FleetCollection.Empty());
		}

		private void Child_Fetch(ScenarioData data)
		{
			LoadProperty<string>(NameProperty, data.Name);
		}
	}
}
