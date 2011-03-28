using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingColors
{
	public class FirePower
	{
		public int Value { get; private set; }
		public bool IsShaded { get; private set; }

		private FirePower(int value, bool isShaded)
		{
			Value = value;
			IsShaded = false;
		}
		private FirePower(int value) : this(value, false)
		{			
		}

		public static FirePower GetBase(RelativeRate rate, int range)
		{

		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}
