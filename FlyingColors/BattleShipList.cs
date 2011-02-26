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
		internal static BattleShipList New(BattleGroupData group, IEnumerable<FleetData> fleets)
		{
			return DataPortal.CreateChild<BattleShipList>(new Tuple<BattleGroupData, IEnumerable<FleetData>>(group, fleets));
		}

		internal static BattleShipList GetShips(BattleGroupData group, IGrouping<string, FleetData> fleets)
		{
			return DataPortal.FetchChild<BattleShipList>(new Tuple<BattleGroupData, IEnumerable<FleetData>>(group, fleets));
		}

		private BattleShipList()
		{
		}

		#region Data Portal

		private IList<BattleShipData> _ships = null;

		private void Child_Create(Tuple<BattleGroupData, IEnumerable<FleetData>> tuple)
		{
			RaiseListChangedEvents = false;
			_ships = tuple.Item1.Ships;
			foreach (var fleet in tuple.Item2)
			{
				foreach (var ship in fleet.Ships)
				{
					Add(BattleShip.NewBattleShip(fleet, ship));
				}
			}
			RaiseListChangedEvents = true;
		}

		private void Child_Fetch(Tuple<BattleGroupData, IEnumerable<FleetData>> tuple)
		{
			RaiseListChangedEvents = false;
			_ships = tuple.Item1.Ships;
			foreach (var ship in _ships)
			{
				Add(BattleShip.GetShip(ship));
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
