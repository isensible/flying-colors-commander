using System;
using System.Collections.Generic;
using Csla;
using FlyingColors.DataModel;

namespace FlyingColors
{
	[Serializable]
	public class FleetList : BusinessListBase<FleetList, Fleet>
	{
		private IList<FleetData> _fleets = null;

		public static FleetList NewFleetList(IList<FleetData> fleets)
		{
			var fleetList = DataPortal.CreateChild<FleetList>();
			fleetList._fleets = fleets;
			return fleetList;
		}

		internal IList<FleetData> ToData(ScenarioData scenarioData)
		{
			foreach (Fleet deletedFleet in DeletedList)
			{
				_fleets.Remove(deletedFleet.ToData(scenarioData));
			}
			foreach (Fleet fleet in this)
			{
				FleetData shipData = fleet.ToData(scenarioData);
				if (_fleets.Contains(shipData))
				{
					int index = _fleets.IndexOf(shipData);
					_fleets[index] = shipData;
				}
				else
				{
					_fleets.Add(shipData);
				}
			}
			return _fleets;
		}
	}
}
