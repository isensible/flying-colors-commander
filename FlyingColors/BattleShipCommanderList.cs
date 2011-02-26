using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using FlyingColors.DataModel;

namespace FlyingColors
{
	[Serializable]
	public class BattleShipCommanderList : BusinessListBase<BattleShipCommanderList, BattleShipCommander>
	{
		internal static BattleShipCommanderList New(BattleShipData ship, IList<FleetShipCommanderData> fleetShipCommanderList)
		{
			return DataPortal.CreateChild<BattleShipCommanderList>(
				new Tuple<BattleShipData, IList<FleetShipCommanderData>>(ship, fleetShipCommanderList));
		}

		internal static BattleShipCommanderList GetCommanders(IList<BattleShipCommanderData> commanders)
		{
			return DataPortal.FetchChild<BattleShipCommanderList>(commanders);
		}

		#region Data Portal

		private IList<BattleShipCommanderData> _commanders = null;

		private void Child_Create(Tuple<BattleShipData, IList<FleetShipCommanderData>> tuple)
		{
			RaiseListChangedEvents = false;
			_commanders = tuple.Item1.Commanders;
			foreach (var commander in tuple.Item2)
			{
				Add(BattleShipCommander.NewCommander(commander));
			}
			RaiseListChangedEvents = true;
		}

		private void Child_Fetch(IList<BattleShipCommanderData> commanders)
		{
			_commanders = commanders;
			RaiseListChangedEvents = false;
			foreach (var commander in commanders)
			{
				Add(BattleShipCommander.GetCommander(commander));
			}
			RaiseListChangedEvents = true;
		}

		internal IList<BattleShipCommanderData> ToData(BattleShipData ship)
		{
			if (_commanders == null)
			{
				_commanders = ship.Commanders;
			}
			foreach (var deletedCommander in DeletedList)
			{
				_commanders.Remove(deletedCommander.ToData(ship));
			}
			foreach (var commander in this)
			{
				var commanderData = commander.ToData(ship);
				if (_commanders.Contains(commanderData))
				{
					int index = _commanders.IndexOf(commanderData);
					_commanders[index] = commanderData;
				}
				else
				{
					_commanders.Add(commanderData);
				}
			}
			return _commanders;
		}

		#endregion

		
	}
}
