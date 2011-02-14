using System;
using System.Collections.Generic;
using Csla;
using FlyingColors.DataModel;

namespace FlyingColors
{
	/// <summary>
	/// Child of Fleet
	/// </summary>
	[Serializable]
	public class FleetShipList : BusinessListBase<FleetShipList, FleetShip>
	{
		private IList<FleetShipData> _fleetShipsDataList;

		public static FleetShipList NewFleetShipList(IList<FleetShipData> fleetShipsDataList)
		{
			var fleetShips = DataPortal.CreateChild<FleetShipList>();
			fleetShips._fleetShipsDataList = fleetShipsDataList;
			return fleetShips;
		}

		internal IList<FleetShipData> ToData(FleetData fleetData)
		{
			foreach (FleetShip deletedShip in DeletedList)
			{
				_fleetShipsDataList.Remove(deletedShip.ToData(fleetData));
			}
			foreach (FleetShip ship in this)
			{
				FleetShipData shipData = ship.ToData(fleetData);
				if (_fleetShipsDataList.Contains(shipData))
				{
					int index = _fleetShipsDataList.IndexOf(shipData);
					_fleetShipsDataList[index] = shipData;
				}
				else
				{
					_fleetShipsDataList.Add(shipData);
				}
			}
			return _fleetShipsDataList;
		}
	}
}
