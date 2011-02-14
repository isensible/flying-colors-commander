using System;
using Castle.ActiveRecord;

namespace FlyingColors.DataModel
{
	[Serializable]
	[ActiveRecord("FleetShip")]
	public class FleetShipData
	{
		[PrimaryKey]
		public long FleetShipId { get; set; }

		[BelongsTo(Type = typeof(FleetData), Column = "FleetId")]
		public FleetData Fleet { get; set; }

		[BelongsTo(Type = typeof(ShipData), Column = "ShipId")]
		public ShipData Ship { get; set; }
	}
}
