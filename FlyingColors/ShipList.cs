using System;
using Csla;
using Castle.ActiveRecord;
using FlyingColors.DataModel;

namespace FlyingColors
{
	[Serializable]
	public class ShipList : DynamicListBase<Ship>
	{
		public static ShipList GetAllShips()
		{
			return DataPortal.Fetch<ShipList>();
		}

		private void DataPortal_Fetch()
		{
			RaiseListChangedEvents = false;
			var ships = ActiveRecordMediator<ShipData>.FindAll();
			foreach (var ship in ships)
			{
				Add(Ship.GetShip(ship));
			}
			RaiseListChangedEvents = true;
		}
	}
}
