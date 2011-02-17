using System;
using Castle.ActiveRecord;

namespace FlyingColors.DataModel
{
	[Serializable]
	[ActiveRecord("Battle")]
	public class BattleData
	{
		[PrimaryKey]
		public long BattleId { get; set; }

		[Property]
		public string Summary { get; set; }

		[Property]
		public DateTime Started { get; set; }

		[Property]
		public DateTime Finished { get; set; }

		[Property]
		public int Turn { get; set; }

		[Property]
		public Weather Weather { get; set; }

		[Property]
		public int WindDirection { get; set; }

		[Property]
		public string Victor { get; set; }

		[BelongsTo(Type = typeof(ScenarioData), Column = "ScenarioId")]
		public ScenarioData Scenario { get; set; }

		public BattleData()
		{
		}
	}
}
