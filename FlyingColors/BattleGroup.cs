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

		internal static BattleGroup NewBattleGroup(Nationality nationality)
		{
			return DataPortal.CreateChild<BattleGroup>(nationality);
		}

		private void Child_Create(Nationality nationality)
		{
			Nationality = nationality;
		}

		internal void AddFleet(Fleet fleet)
		{
			foreach (var ship in fleet.Ships)
			{
				Add(BattleShip.NewBattleShip(ship));
			}
		}
	}
}
