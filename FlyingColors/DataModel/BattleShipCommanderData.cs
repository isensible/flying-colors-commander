using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace FlyingColors.DataModel
{
	[Serializable]
	[ActiveRecord("BattleShipCommander")]
	public class BattleShipCommanderData
	{
		[PrimaryKey]
		public long BattleShipCommanderId { get; set; }

		[BelongsTo(Type = typeof(CommanderData), Column = "CommanderId")]
		public CommanderData Commander { get; set; }

		[BelongsTo(Type = typeof(BattleShipData), Column = "BattleShipId")]
		public BattleShipData Ship { get; set; }

		[Property]
		public bool Wounded { get; set; }

		[Property]
		public bool Killed { get; set; }

		[Property]
		public bool Captured { get; set; }
	}
}
