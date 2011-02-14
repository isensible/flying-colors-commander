using System;
using System.Collections.Generic;
using Castle.ActiveRecord;

namespace FlyingColors.DataModel
{
	[Serializable]
	[ActiveRecord("Scenario")]
	public class ScenarioData
	{
		[PrimaryKey]
		public long ScenarioId { get; set; }

		[Property]
		public string Name { get; set; }

		[Property]
		public int Year { get; set; }

		[Property]
		public int WindDirection { get; set; }

		[Property]
		public Weather WeatherEffects { get; set; }

		[Property]
		public string Maps { get; set; }

		[Property]
		public int Turns { get; set; }

		[HasMany(typeof(FleetData),
			Table = "Fleet",
			ColumnKey = "ScenarioId",
			Cascade = ManyRelationCascadeEnum.All)]
		public IList<FleetData> Fleets { get; set; }

		public ScenarioData()
		{
			Fleets = new List<FleetData>();
		}
	}
}
