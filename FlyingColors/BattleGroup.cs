using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;

namespace FlyingColors
{
	[Serializable]
	public class BattleGroup : BusinessListBase<BattleGroup, BattleShip>
	{
		public Nationality Nationality { get; private set; }

		internal static BattleGroup NewBattleGroup(IGrouping<Nationality, Fleet> fleets)
		{
			return DataPortal.CreateChild<BattleGroup>(fleets);
		}

		private void Child_Create(IGrouping<Nationality, Fleet> fleets)
		{
			RaiseListChangedEvents = false;
			this.Nationality = fleets.Key;
			foreach (var fleet in fleets)
			{
				foreach (var ship in fleet.Ships)
				{
					Add(BattleShip.NewBattleShip(fleet, ship));
				}
			}
			RaiseListChangedEvents = true;
		}

		internal void AddFleet(Fleet fleet)
		{
			RaiseListChangedEvents = false;
			foreach (var ship in fleet.Ships)
			{
				Add(BattleShip.NewBattleShip(fleet, ship));
			}
			RaiseListChangedEvents = true;
		}
	}
}
