using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace FlyingColors.DataModel
{
	[Serializable]
	[ActiveRecord("FleetShipCommander")]
	public class FleetShipCommanderData
	{
		[PrimaryKey]
		public long FleetShipCommanderId { get; set; }

		[BelongsTo(Type = typeof(FleetShipData), Column = "FleetShipId")]
		public FleetShipData FleetShip { get; set; }

		[BelongsTo(Type = typeof(CommanderData), Column = "CommanderId")]
		public CommanderData Commander { get; set; }
	}
}
