using System;
using Castle.ActiveRecord;
using Csla;
using FlyingColors.DataModel;

namespace FlyingColors
{
	/// <summary>
	/// Child of FleetShipList
	/// </summary>
	[Serializable]
	public class FleetShip : BusinessBase<FleetShip>
	{
		#region The Ship
		public static readonly PropertyInfo<Ship> ShipProperty = RegisterProperty<Ship>(c => c.Ship);
		/// <Summary>
		/// Gets or sets the Ship value.
		/// </Summary>
		public Ship Ship
		{
			get { return GetProperty(ShipProperty); }
			set { SetProperty(ShipProperty, value); }
		} 
		#endregion

		#region Hull and Rigging Hits at start of scenario
		public static readonly PropertyInfo<int> HullHitsAtStartProperty = RegisterProperty<int>(c => c.HullHitsAtStart);
		/// <Summary>
		/// Gets or sets the HullHitsAtStart value.
		/// </Summary>
		public int HullHitsAtStart
		{
			get { return GetProperty(HullHitsAtStartProperty); }
			set { SetProperty(HullHitsAtStartProperty, value); }
		}

		public static readonly PropertyInfo<int> RiggingHitsAtStartProperty = RegisterProperty<int>(c => c.RiggingHitsAtStart);
		/// <Summary>
		/// Gets or sets the RiggingHitsAtStart value.
		/// </Summary>
		public int RiggingHitsAtStart
		{
			get { return GetProperty(RiggingHitsAtStartProperty); }
			set { SetProperty(RiggingHitsAtStartProperty, value); }
		} 
		#endregion

		#region Factory Methods
		public static FleetShip NewFleetShip(Ship ship)
		{
			return DataPortal.CreateChild<FleetShip>(ship);
		}

		internal static FleetShip GetFleetShip(FleetShipData fleetShip)
		{
			return DataPortal.FetchChild<FleetShip>(fleetShip);
		}

		private FleetShip()
		{
		} 
		#endregion

		#region Data Portal
		private FleetShipData _fleetShipData = null;

		private void Child_Create(Ship ship)
		{
			LoadProperty<Ship>(ShipProperty, ship);
			LoadProperty<int>(HullHitsAtStartProperty, 0);
			LoadProperty<int>(RiggingHitsAtStartProperty, 0);
		}

		private void Child_Fetch(FleetShipData fleetShipData)
		{
			_fleetShipData = fleetShipData;
			LoadProperty<Ship>(ShipProperty, Ship.GetShip(fleetShipData.Ship));
			LoadProperty<int>(HullHitsAtStartProperty, fleetShipData.HullHitsAtStart);
			LoadProperty<int>(RiggingHitsAtStartProperty, fleetShipData.RiggingHitsAtStart);
		}

		private void Child_Insert(FleetData fleetData)
		{
			ActiveRecordMediator<FleetShipData>.Create(ToData(fleetData));
		}

		private void Child_Update(FleetData fleetData)
		{
			ActiveRecordMediator<FleetShipData>.Update(ToData(fleetData));
		}

		private void Child_Delete(FleetData fleetData)
		{
			ActiveRecordMediator<FleetShipData>.Delete(ToData(fleetData));
		}

		internal FleetShipData ToData(FleetData fleetData)
		{
			if (_fleetShipData == null)
			{
				_fleetShipData = new FleetShipData();
			}
			_fleetShipData.Fleet = fleetData;
			_fleetShipData.Ship = ReadProperty<Ship>(ShipProperty).ToData();
			_fleetShipData.HullHitsAtStart = ReadProperty<int>(HullHitsAtStartProperty);
			_fleetShipData.RiggingHitsAtStart = ReadProperty<int>(RiggingHitsAtStartProperty);
			return _fleetShipData;
		} 
		#endregion		
	}
}
