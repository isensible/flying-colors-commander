using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace FlyingColors.DataModel
{
	[Serializable]
	[ActiveRecord("BattleShip")]
	public class BattleShipData
	{
		[PrimaryKey]
		public long BattleShipId { get; set; }

		[BelongsTo(Type = typeof(BattleGroupData), Column = "BattleGroupId")]
		public BattleGroupData BattleGroup { get; set; }

		[BelongsTo(Type = typeof(FleetShipData), Column = "FleetShipId")]
		public FleetShipData FleetShip { get; set; }

		[HasMany(typeof(BattleShipCommanderData),
			Table = "BattleShipCommander",
			ColumnKey = "BattleShipCommanderId",
			Cascade = ManyRelationCascadeEnum.All)]
		public IList<BattleShipCommanderData> Commanders { get; set; }

		[Property]
		public bool FiredPort { get; set; }
		[Property]
		public bool FirstPortFired { get; set; }
		[Property]
		public bool FiredStarboard { get; set; }
		[Property]
		public bool FirstStarboardFired { get; set; }
		[Property]
		public int HullHits { get; set; }
		[Property]
		public int RiggingHits { get; set; }
		[Property]
		public int MarineHits { get; set; }
		[Property]
		public bool OutOfCommand { get; set; }
		[Property]
		public int CommandGroup { get; set; }
		[Property]
		public bool InIrons { get; set; }
		[Property]
		public bool Adrift { get; set; }
		[Property]
		public bool Anchored { get; set; }
		[Property]
		public bool FullSails { get; set; }
		[Property]
		public bool Aground { get; set; }
		[Property]
		public bool OnFire { get; set; }
		[Property]
		public bool Struck { get; set; }
		[Property]
		public bool Captured { get; set; }
		[Property]
		public bool Moved { get; set; }
		[Property]
		public bool Tacked { get; set; }

		public BattleShipData()
		{
			Commanders = new List<BattleShipCommanderData>();
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj is BattleShipData)
			{
				var other = (BattleShipData)obj;
				return (this.FleetShip.FleetShipId == other.FleetShip.FleetShipId);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return FleetShip.FleetShipId.GetHashCode();
		}

		public override string ToString()
		{
			return FleetShip.Ship.Name;
		}
	}
}
