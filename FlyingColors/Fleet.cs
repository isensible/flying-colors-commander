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

		public static readonly PropertyInfo<Nationality> BattleGroupProperty = RegisterProperty<Nationality>(c => c.BattleGroup);
		/// <Summary>
		/// Gets or sets the BattleGroup value.
		/// </Summary>
		public Nationality BattleGroup
		{
			get { return GetProperty(BattleGroupProperty); }
			set { SetProperty(BattleGroupProperty, value); }
		}

		public static readonly PropertyInfo<Nationality> NationalityProperty = RegisterProperty<Nationality>(c => c.Nationality);
		/// <Summary>
		/// Gets or sets the Nationality value.
		/// </Summary>		
		public Nationality Nationality
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

		internal static Fleet GetFleet(FleetData fleet)
		{
			return DataPortal.FetchChild<Fleet>(fleet);
		}

		protected override void Child_Create()
		{
			_fleet = new FleetData();
			LoadProperty<Nationality>(BattleGroupProperty, Nationality.British);
			LoadProperty<Nationality>(NationalityProperty, Nationality.British);
			LoadProperty<int>(AudacityProperty, 1);
			LoadProperty<FleetShipList>(ShipsProperty, FleetShipList.NewFleetShipList(_fleet.Ships));
		}

		private void Child_Fetch(FleetData fleet)
		{
			_fleet = fleet;
			LoadProperty<Nationality>(BattleGroupProperty, (Nationality)Enum.Parse(typeof(Nationality), _fleet.BattleGroup));
			LoadProperty<Nationality>(NationalityProperty, (Nationality)Enum.Parse(typeof(Nationality), _fleet.Nationality));
			LoadProperty<int>(AudacityProperty, _fleet.Audacity);
			LoadProperty<FleetShipList>(ShipsProperty, FleetShipList.GetFleetShipList(_fleet.Ships));
		}

		private void Child_Insert(ScenarioData scenarioData)
		{
			ToData(scenarioData);
			FieldManager.UpdateChildren(_fleet);
			ActiveRecordMediator<FleetData>.Create(_fleet);
			LoadProperty<long>(FleetIdProperty, _fleet.FleetId);
		}

		private void Child_Update(ScenarioData scenarioData)
		{
			ToData(scenarioData);
			FieldManager.UpdateChildren(_fleet);
			ActiveRecordMediator<FleetData>.Update(_fleet);
		}

		private void Child_Delete(ScenarioData scenarioData)
		{
			ToData(scenarioData);
			FieldManager.UpdateChildren(_fleet);
			ActiveRecordMediator<FleetData>.Delete(_fleet);
		}

		internal FleetData ToData(ScenarioData scenarioData)
		{
			_fleet.Scenario = scenarioData;
			_fleet.BattleGroup = ReadProperty<Nationality>(BattleGroupProperty).ToString();
			_fleet.Nationality = ReadProperty<Nationality>(NationalityProperty).ToString();
			_fleet.Audacity = ReadProperty<int>(AudacityProperty);
			_fleet.Ships = Ships.ToData(_fleet);
			return _fleet;
		}


	}
}
