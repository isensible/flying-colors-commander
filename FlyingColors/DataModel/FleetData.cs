using System;
using System.Collections.Generic;
using Castle.ActiveRecord;

namespace FlyingColors.DataModel
{
	[Serializable]
	[ActiveRecord("Fleet")]
	public class FleetData
	{
		[PrimaryKey]
		public long FleetId { get; set; }

		[BelongsTo(Type = typeof(ScenarioData), Column = "ScenarioId")]
		public ScenarioData Scenario { get; set; }

		[Property]
		public string BattleGroup { get; set; }

		[Property]
		public string Nationality { get; set; }

		[Property]
		public int Audacity { get; set; }

		[HasMany(typeof(FleetShipData),
			Table = "FleetShip",
			ColumnKey = "FleetId",
			Cascade = ManyRelationCascadeEnum.All)]
		public IList<FleetShipData> Ships { get; set; }

		public FleetData()
		{
			Ships = new List<FleetShipData>();
		}
	}
}
