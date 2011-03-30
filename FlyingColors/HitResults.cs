using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingColors
{
	public class HitResults
	{
		// map fire-power to list of damage ordered by die-roll
		public readonly static Dictionary<int, string[]> Rigging = null;
		public readonly static Dictionary<int, string[]> Hull = null;

		public readonly static Dictionary<int, string[]> SmallVesselRigging = null;
		public readonly static Dictionary<int, string[]> SmallVesselHull = null;

		static HitResults()
		{
			Rigging = new Dictionary<int, string[]>(23);
			Hull = new Dictionary<int, string[]>(23);
			SmallVesselRigging = new Dictionary<int, string[]>(23);
			SmallVesselHull = new Dictionary<int, string[]>(23);

			PopulateTable(Rigging, riggingResults);
		}

		private readonly static string riggingResults =
			// Die Roll
			//  0,   1,   2,   3,   4,   5,   6,   7,   8,   9,  10,  11,  12;    // Fire Power
			"   -,   -,   -,   -,   -,   -,   -,   R,   R,   H,   R,   M,  2R;" + // <0
			"   -,   -,   -,   -,   -,   -,   R,   R,   H,  2R,   M,  2R,  R*;" + //  0
			"   -,   -,   -,   -,   -,   R,   R,   H,   R,   M,  2R,  R*,  RH;";  //  1


		private static void PopulateTable(Dictionary<int, string[]> table, string resultString)
		{
			string[] lines = resultString.Split(';');
			int firePower = -1;
			foreach (var line in lines)
			{
				string[] results = line.Split(',');
				table.Add(firePower, results);
				firePower++;
			}
		}

		public static Damage GetRiggingDamage(FirePower firePower, int dieRoll)
		{
			return new Damage(Rigging[firePower.Value][dieRoll].Trim());
		}

		public static Damage GetHullDamage(FirePower firePower, int dieRoll)
		{
			return new Damage(Hull[firePower.Value][dieRoll].Trim());
		}

		public static Damage GetSmallVesselRiggingDamage(FirePower firePower, int dieRoll)
		{
			return new Damage(SmallVesselRigging[firePower.Value][dieRoll].Trim());
		}

		public static Damage GetSmallVesselHullDamage(FirePower firePower, int dieRoll)
		{
			return new Damage(SmallVesselHull[firePower.Value][dieRoll].Trim());
		}
	}
}
