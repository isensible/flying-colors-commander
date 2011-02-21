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
		internal static BattleShipCommanderList New(IList<FleetShipCommanderData> fleetShipCommanderList)
		{
			return DataPortal.CreateChild<BattleShipCommanderList>(fleetShipCommanderList);
		}

		#region Data Portal

		private IList<BattleShipCommanderData> _commanders = null;

		private void Child_Create(IList<FleetShipCommanderData> commanders)
		{
			RaiseListChangedEvents = false;
			foreach (var commander in commanders)
			{
				Add(BattleShipCommander.NewCommander(commander));
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
