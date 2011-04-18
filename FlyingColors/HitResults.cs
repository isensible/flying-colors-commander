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
			PopulateTable(Hull, hullResults);
		}

		private readonly static string riggingResults =
			// Die Roll
			//  0,   1,   2,   3,   4,   5,   6,   7,   8,   9,  10,  11,  12;    // Fire Power
			"   -,   -,   -,   -,   -,   -,   -,   R,   R,   H,   R,   M,  2R;" + // <0
			"   -,   -,   -,   -,   -,   -,   R,   R,   H,  2R,   M,  2R,  R*;" + //  0
			"   -,   -,   -,   -,   -,   R,   R,   H,   R,   M,  2R,  R*,  RH;" + //  1
			"   -,   -,   -,   -,   R,   R,   H,   R,   M,  2R,  R*,  RH,  2R;" + //  2
			"   -,   -,   -,   R,   R,   H,   R,   M,  2R,  R*,  RH,  2R,  2R;" + //  3
			"   -,   -,   R,   R,   H,   R,   M,  2R,  R*,  RH,  2R,  2R,  3R;" + //  4
			"   -,   R,   R,   H,   R,   M,  2R,  R*, 2RH,  2R,  2R,  3R, 2R*;" + //  5
			"   R,   R,   H,   R,   M,  2R,  R*, 2RH,  2R,  3R,  3R, 2R*, 2RH;" + //  6
			"   R,   H,   R,   M,  2R,  R*, 2RH,  2R,  3R,  4R, 3R*, 3RH,  4R;" + //  7
			"   H,   R,   M,  2R, 2R*, 2RH,  2R,  3R,  4R, 3R*, 3RH,  4R, 3RM;" + //  8
			"   R,   M,  2R, 2R*, 2RH,  2R,  3R,  4R, 3R*, 3RH,  4R, 3RM,  5R;" + //  9
			"   M,  2R, 2R*, 2RH,  3R,  2RM, 3R, 3R*, 3RH,  4R, 3RM,  5R, 4R*;" + // 10
			"  2R, 2R*, 2RH,  3R,  3R,  3R, 3R*, 3RH, 4R*, 3RM,  5R, 4R*, 4RH;" + // 11
			" 2R*, 2RH,  3R,  3R,  4R, 3R*, 3RH, 4R*, 3RM,  5R, 4R*, 4RH,  5R;" + // 12
			" 2RH,  3R,  3R,  4R, 3R*, 3RH,  4R, 4RM,  5R, 5R*, 4RH,  5R, 4RM;" + // 13
			"  3R,  3R,  4R, 3R*, 3RH,  4R, 4RM,  6R, 5R*, 5RH,  6R, 5RM,  7R;" + // 14
			"  3R,  4R, 3R*, 3RH,  4R, 4RM,  6R, 5R*, 5RH,  6R,  6R,  7R, 6R*;" + // 15
			"  4R, 3R*, 4RH,  5R, 4RM,  6R, 5R*, 5RH,  6R,  6R,  7R, 6R*,5R2H;" + // 16
			" 4R*, 4RH,  5R, 4RM,  6R, 6R*, 5RH,  6R,  6R,  7R, 6R*,5R2H, 6RH;" + // 17
			" 4RH,  5R, 4RM,  6R, 6R*, 5RH,  6R,  7R,  7R, 6R*,5R2H, 6RH, 6RM;" + // 18
			"  5R, 5RM,  6R, 6R*, 5RH,  6R,  7R,  7R, 6R*,5R2H, 6RH, 6RM,  8R;" + // 19
			" 6RM,  7R, 6R*, 6RH,  6R,  7R,  8R, 7R*,5R2H, 6RH, 7RM,  9R, 8R*;" + // 20
			"  7R, 7R*, 6RH,  7R,  7R,  8R, 7R*,6R2H, 7RH, 7RM,  9R, 8R*,7R2H";   // 20>

		private readonly static string hullResults =
			// Die Roll
			//  0,   1,   2,   3,   4,   5,   6,   7,   8,   9,  10,  11,  12;    // Fire Power
			"   -,   -,   -,   -,   -,   -,   -,   -,   -,   H,   H,   R,   H;" + // <0
			"   -,   -,   -,   -,   -,   -,   -,   -,   H,   H,   R,   H,   M;" + //  0
			"   -,   -,   -,   -,   -,   -,   -,   H,   H,   R,   H,   M,  HR;" + //  1
			"   -,   -,   -,   -,   -,   -,   H,   H,   R,   H,   M,  2H,  2H;" + //  2
			"   -,   -,   -,   -,   -,   H,   H,   R,   H,   M,  2H,  2H,  HR;" + //  3
			"   -,   -,   -,   -,   H,   H,   R,   H,   M,  2H,  2H,  HR,  2H;" + //  4
			"   -,   -,   -,   H,   H,   R,   H,   M,  2H,  2H, 2HR,  2H,  2H;" + //  5
			"   -,   -,   H,   H,   R,   H,   M,  2H,  2H, 2HR,  2H,  3H,  3H;" + //  6
			"   -,   H,   H,   R,   H,   M,  HR,   H, 2HR,  2H,  3H,  4H, 3H*;" + //  7
			"   H,   H,   R,   H,   M,  HR,  2H, 2HR,  2H,  3H,  4H, 3H*, 3HR;" + //  8
			"   H,   R,   H,   M,  HR,  2H, 2HR,  2H,  3H,  4H, 3H*, 3HR,  4H;" + //  9
			"   H,   R,   M,  HR,  2H, 2HR,  3H, 2HM,  3H, 3H*, 3HR,  4H, 3HM;" + // 10
			"   R,   M,  HR,  2H, 2HR,  3H,  3H,  3H, 3H*, 3HR, 4H*, 3HM,  5H;" + // 11
			"   M,  HR,  2H, 2HR,  3H,  3H,  4H, 3H*, 3HR, 4H*, 3HM,  5H, 4H*;" + // 12
			"  HR,  2H, 2HR,  3H,  3H,  4H, 3H*, 3HR,  4H, 4HM,  5H, 5H*, 4HR;" + // 13
			"  2H, 2HR,  3H,  3H,  4H, 3H*, 3HR,  4H, 4HM,  6H, 5H*, 5HR,  6H;" + // 14
			" 2HR,  3H,  3H,  4H, 3H*, 3HR,  4H, 4HM,  6H, 5H*, 5HR,  6H,  6H;" + // 15
			"  3H,  3H,  4H, 3H*, 4HR,  5H, 4HM,  6H, 5H*, 5HR,  6H,  6H,  7H;" + // 16
			"  3H,  4H, 3H*, 4HR,  5H, 4HM,  6H, 5H*, 5HR,  6H,  6H,  7H, 6H*;" + // 17
			"  4H, 4H*, 4HR,  5H, 4HM,  6H, 6H*, 5HR,  6H,  7H,  7H, 6H*,2H2R;" + // 18
			" 4H*, 4HR,  5H, 5HM,  6H, 6H*, 5HR,  6H,  7H,  7H, 6H*,2H2R, 6HR;" + // 19
			" 5HM,  6H, 6HM,  7H, 6H*, 6HR,  6H,  7H,  8H, 7H*,5H2R, 6HR, 7HM;" + // 20
			"  6H, 6HM,  7H, 7H*, 6HR,  7H,  7H,  8H, 7H*,6H2R, 7HR, 7HM,  9H";   // 20>

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
