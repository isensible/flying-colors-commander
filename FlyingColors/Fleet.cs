using System;
using Castle.ActiveRecord;
using Csla;
using FlyingColors.DataModel;

namespace FlyingColors
{
	[Serializable]
	public class Fleet : BusinessBase<Fleet>
	{
		private FleetData _fleet = null;

		public static readonly PropertyInfo<long> FleetIdProperty = RegisterProperty<long>(c => c.FleetId);
		/// <Summary>
		/// Gets or sets the FleetId value.
		/// </Summary>		
		public virtual long FleetId
		{
			get { return GetProperty(FleetIdProperty); }
			set { SetProperty(FleetIdProperty, value); }
		}

		public static readonly PropertyInfo<string> TeamProperty = RegisterProperty<string>(c => c.Team);
		/// <Summary>
		/// Gets or sets the Team value.
		/// </Summary>
		public string Team
		{
			get { return GetProperty(TeamProperty); }
			set { SetProperty(TeamProperty, value); }
		}

		public static readonly PropertyInfo<string> NationalityProperty = RegisterProperty<string>(c => c.Nationality);
		/// <Summary>
		/// Gets or sets the Nationality value.
		/// </Summary>		
		public string Nationality
		{
			get { return GetProperty(NationalityProperty); }
			set { SetProperty(NationalityProperty, value); }
		}

		public static readonly PropertyInfo<int> AudacityProperty = RegisterProperty<int>(c => c.Audacity);
		/// <Summary>
		/// Gets or sets the Audacity value.
		/// </Summary>		
		public int Audacity
		{
			get { return GetProperty(AudacityProperty); }
			set { SetProperty(AudacityProperty, value); }
		}

		public static readonly PropertyInfo<FleetShipList> ShipsProperty =
			RegisterProperty<FleetShipList>(c => c.Ships);

		/// <Summary>
		/// Gets or sets the Ships value.
		/// </Summary>		
		public FleetShipList Ships
		{
			get { return GetProperty(ShipsProperty); }
			set { SetProperty(ShipsProperty, value); }
		}

		public static Fleet NewFleet()
		{
			return DataPortal.CreateChild<Fleet>();
		}

		protected override void Child_Create()
		{
			_fleet = new FleetData();
			LoadProperty<string>(TeamProperty, "English");
			LoadProperty<string>(NationalityProperty, "English");
			LoadProperty<int>(AudacityProperty, 1);
			LoadProperty<FleetShipList>(ShipsProperty, FleetShipList.NewFleetShipList(_fleet.Ships));
		}

		private void Child_Insert(ScenarioData scenarioData)
		{
			FieldManager.UpdateChildren(this.ToData(scenarioData));
			ActiveRecordMediator<FleetData>.Create(ToData(scenarioData));
		}

		private void DataPortal_Update(ScenarioData scenarioData)
		{
			FieldManager.UpdateChildren(this.ToData(scenarioData));
			ActiveRecordMediator<FleetData>.Update(ToData(scenarioData));
		}

		internal FleetData ToData(ScenarioData scenarioData)
		{
			_fleet.Scenario = scenarioData;
			_fleet.Team = ReadProperty<string>(TeamProperty);
			_fleet.Nationality = ReadProperty<string>(NationalityProperty);
			_fleet.Audacity = ReadProperty<int>(AudacityProperty);
			_fleet.Ships = Ships.ToData(_fleet);
			return _fleet;
		}
	}
}
