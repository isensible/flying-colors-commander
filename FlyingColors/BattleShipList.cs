using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using FlyingColors.DataModel;

namespace FlyingColors
{
	[Serializable]
	public class BattleShipList : BusinessListBase<BattleShipList, BattleShip>
	{
		internal static BattleShipList New(IEnumerable<FleetData> fleets)
		{
			return DataPortal.CreateChild<BattleShipList>(fleets);
		}

		private BattleShipList()
		{
		}

		#region Data Portal

		private IList<BattleShipData> _ships = null;

		private void Child_Create(IEnumerable<FleetData> fleets)
		{
			RaiseListChangedEvents = false;
			foreach (var fleet in fleets)
			{
				foreach (var ship in fleet.Ships)
				{
					Add(BattleShip.NewBattleShip(fleet, ship));
				}
			}
			RaiseListChangedEvents = true;
		}

		internal IList<BattleShipData> ToData(BattleGroupData group)
		{
			if (_ships == null)
			{
				_ships = group.Ships;
			}
			foreach (var deletedShip in DeletedList)
			{
				_ships.Remove(deletedShip.ToData(group));
			}
			foreach (var ship in this)
			{
				var shipData = ship.ToData(group);
				if (_ships.Contains(shipData))
				{
					int index = _ships.IndexOf(shipData);
					_ships[index] = shipData;
				}
				else
				{
					_ships.Add(shipData);
				}
			}
			return _ships;
		}

		#endregion
	}
}
