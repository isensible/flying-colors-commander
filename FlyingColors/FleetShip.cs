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
		private FleetShipData _fleetShipData = null;

		public static readonly PropertyInfo<Ship> ShipProperty = RegisterProperty<Ship>(c => c.Ship);
		/// <Summary>
		/// Gets or sets the Ship value.
		/// </Summary>
		public Ship Ship
		{
			get { return GetProperty(ShipProperty); }
			set { SetProperty(ShipProperty, value); }
		}

		public static FleetShip NewFleetShip(Ship ship)
		{
			return DataPortal.CreateChild<FleetShip>(ship);
		}

		private void Child_Create(Ship ship)
		{
			LoadProperty<Ship>(ShipProperty, ship);
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
			return _fleetShipData;
		}
	}
}
