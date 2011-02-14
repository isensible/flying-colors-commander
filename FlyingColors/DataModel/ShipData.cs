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
	}
}
