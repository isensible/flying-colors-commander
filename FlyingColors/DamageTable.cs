using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingColors
{
	public class FirePower
	{
		public int Value { get; set; }
	}

	public class DieRoll
	{
		public int Value { get; set; }
	}	

	public static class DamageTable
	{
		public static Dictionary<FirePower, Dictionary<DieRoll, Damage>> DamageTable = new Dictionary<FirePower, Dictionary<DieRoll, Damage>>();
 
	}
}
