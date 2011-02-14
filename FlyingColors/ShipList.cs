using System;
using Csla;

namespace FlyingColors
{
	[Serializable]
	public class ShipList : DynamicListBase<Ship>
	{
		internal static ShipList NewShipList()
		{
			var shipList = DataPortal.Create<ShipList>();
			return shipList;
		}
	}
}
