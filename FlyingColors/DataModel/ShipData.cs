using System;
using Castle.ActiveRecord;

namespace FlyingColors.DataModel
{
	[Serializable]
	[ActiveRecord("Ship")]
	public class ShipData
	{
		[PrimaryKey]
		public long ShipId { get; set; }

		[Property]
		public string Name { get; set; }

		[Property]
		public string Nationality { get; set; }

		[Property]
		public int VictoryPoints { get; set; }

		[Property]
		public char Rate { get; set; }

		[Property]
		public string RateColor { get; set; }

		[Property]
		public string RateShape { get; set; }

		[Property]
		public char RateDamaged { get; set; }

		[Property]
		public string RateColorDamaged { get; set; }

		[Property]
		public string RateShapeDamaged { get; set; }

		[Property]
		public int DamageCapacity { get; set; }

		[Property]
		public int DamageCapacityDamaged { get; set; }

		[Property]
		public int Marines { get; set; }

		[Property]
		public int MarinesDamaged { get; set; }		
	}
}
