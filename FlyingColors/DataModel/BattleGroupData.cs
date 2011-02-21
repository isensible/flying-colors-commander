using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace FlyingColors.DataModel
{
	[Serializable]
	[ActiveRecord("BattleGroup")]
	public class BattleGroupData
	{
		[PrimaryKey]
		public long BattleGroupId { get; set; }

		[BelongsTo(Type = typeof(BattleData), Column = "BattleId")]
		public BattleData Battle { get; set; }

		[Property]
		public string Nationality { get; set; }

		[HasMany(typeof(BattleShipData),
			Table = "BattleShip",
			ColumnKey = "BattleGroupId",
			Cascade = ManyRelationCascadeEnum.All, Inverse = true)]
		public IList<BattleShipData> Ships { get; set; }

		public BattleGroupData()
		{
			Ships = new List<BattleShipData>();
		}
	}
}
