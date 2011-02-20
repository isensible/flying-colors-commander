using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;

namespace FlyingColors
{
	[Serializable]
	public class BattleShipCommanderList : BusinessListBase<BattleShipCommanderList, BattleShipCommander>
	{
		internal static BattleShipCommanderList New(FleetShipCommanderList fleetShipCommanderList)
		{
			return DataPortal.CreateChild<BattleShipCommanderList>(fleetShipCommanderList);
		}

		private void Child_Create(FleetShipCommanderList commanders)
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
