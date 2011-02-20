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

		private void Child_Create(IList<FleetShipCommanderData> commanders)
		{
			RaiseListChangedEvents = false;
			foreach (var commander in commanders)
			{
				Add(BattleShipCommander.NewCommander(commander));
			}
			RaiseListChangedEvents = true;
		}
	}
}
