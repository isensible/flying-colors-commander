using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingColors
{
	public class DamageKey
	{
		public int ModifiedDieRoll { get; private set; }
		public int FirePower { get; private set; }

		public DamageKey(int firePower, int modifiedDieRoll)
		{
			FirePower = firePower;
			ModifiedDieRoll = modifiedDieRoll;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj is DamageKey)
			{
				var other = obj as DamageKey;
				return (this.FirePower == other.FirePower) && (this.ModifiedDieRoll == other.ModifiedDieRoll);
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return ((FirePower * 251) + ModifiedDieRoll) * 251;
		}

		public override string ToString()
		{
			return string.Format("Fire power={0}; Die roll={1}.", FirePower, ModifiedDieRoll);
		}
	}
}
