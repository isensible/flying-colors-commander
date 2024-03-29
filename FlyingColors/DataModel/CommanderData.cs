﻿using System;
using Castle.ActiveRecord;

namespace FlyingColors.DataModel
{
	[Serializable]
	[ActiveRecord("Commander")]
	public class CommanderData
	{
		[PrimaryKey]
		public long CommanderId { get; set; }

		[Property]
		public string Name { get; set; }

		[Property]
		public string Nationality { get; set; }

		[Property]
		public int VictoryPoints { get; set; }

		[Property]
		public int Rank { get; set; }

		[Property]
		public int Quality { get; set; }

		[Property]
		public int QualityWounded { get; set; }

		[Property]
		public int Range { get; set; }

		[Property]
		public int RangeWounded { get; set; }
	}
}
