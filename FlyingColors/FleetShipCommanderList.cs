using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using FlyingColors.DataModel;

namespace FlyingColors
{
	[Serializable]
	public class FleetShipCommanderList : BusinessListBase<FleetShipCommanderList, FleetShipCommander>
	{
		#region Factory Methods
		public static FleetShipCommanderList NewFleetShipCommanderList()
		{
			return DataPortal.CreateChild<FleetShipCommanderList>();
		}

		public static FleetShipCommanderList GetFleetShipCommanderList(IList<FleetShipCommanderData> commanders)
		{
			return DataPortal.FetchChild<FleetShipCommanderList>(commanders);
		}

		private FleetShipCommanderList()
		{
		} 
		#endregion

		#region Data Portal
		private IList<FleetShipCommanderData> _commanders = null;

		private void Child_Create()
		{
			_commanders = new List<FleetShipCommanderData>();
		}

		private void Child_Fetch(IList<FleetShipCommanderData> commanders)
		{
			RaiseListChangedEvents = false;
			_commanders = commanders;
			foreach (var commander in _commanders)
			{
				Add(FleetShipCommander.GetFleetShipCommander(commander));
			}
			RaiseListChangedEvents = true;
		}

		internal IList<FleetShipCommanderData> ToData(FleetShipData fleetShipData)
		{
			foreach (FleetShipCommander deletedCommander in DeletedList)
			{
				_commanders.Remove(deletedCommander.ToData(fleetShipData));
			}
			foreach (FleetShipCommander commander in this)
			{
				FleetShipCommanderData commanderData = commander.ToData(fleetShipData);
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
