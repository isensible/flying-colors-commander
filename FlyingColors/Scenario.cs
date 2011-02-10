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

		public static readonly PropertyInfo<FleetCollection> FleetsProperty = RegisterProperty<FleetCollection>(c => c.Fleets);
		/// <Summary>
		/// Gets or sets the Fleets value.
		/// </Summary>
		public FleetCollection Fleets
		{
			get { return GetProperty(FleetsProperty); }
			set { SetProperty(FleetsProperty, value); }
		}

		internal static Scenario NewScenario(string name)
		{
			return DataPortal.CreateChild<Scenario>(name);
		}

		private void Child_Create(string name)
		{
			LoadProperty<string>(NameProperty, name);
			LoadProperty<FleetCollection>(FleetsProperty, FleetCollection.Empty());
		}
	}
}
