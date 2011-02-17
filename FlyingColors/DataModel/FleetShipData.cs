using System;
using System.Collections.Generic;
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

		[HasMany(typeof(FleetShipCommanderData),
			Table = "FleetShipCommander",
			ColumnKey = "FleetShipId",
			Cascade = ManyRelationCascadeEnum.All)]
		public IList<FleetShipCommanderData> Commanders { get; set; }

		[Property]
		public int HullHitsAtStart { get; set; }
		[Property]
		public int RiggingHitsAtStart { get; set; }
		[Property]
		public int MarineHitsAtStart { get; set; }

		[Property]
		public bool AnchoredAtStart { get; set; }
		[Property]
		public bool FullSailsAtStart { get; set; }


		public FleetShipData()
		{
			Commanders = new List<FleetShipCommanderData>();
		}
	}
}
