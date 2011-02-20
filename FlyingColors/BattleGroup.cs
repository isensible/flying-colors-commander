using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using FlyingColors.DataModel;

namespace FlyingColors
{
	[Serializable]
	public class BattleGroup : BusinessListBase<BattleGroup, BattleShip>
	{
		public Nationality Nationality { get; private set; }

		internal static BattleGroup NewBattleGroup(IGrouping<string, FleetData> fleets)
		{
			return DataPortal.CreateChild<BattleGroup>(fleets);
		}

		private void Child_Create(IGrouping<string, FleetData> fleets)
		{
			RaiseListChangedEvents = false;
			this.Nationality = (Nationality)Enum.Parse(typeof(Nationality), fleets.Key);
			foreach (var fleet in fleets)
			{
				foreach (var ship in fleet.Ships)
				{
					Add(BattleShip.NewBattleShip(fleet, ship));
				}
			}
			RaiseListChangedEvents = true;
		}		
	}
}
