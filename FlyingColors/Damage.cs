using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingColors
{
	public class Damage
	{
		public int Hull { get; private set; }
		public int Rigging { get; private set; }
		public int Marines { get; private set; }
		public bool ChanceOfFire { get; private set; }

		/// <summary>
		/// Result label.
		/// </summary>
		public string Result { get; private set; }

		#region Ctor
		public Damage(string result)
		{
			Result = result;
			ParseResult();
		}

		private void ParseResult()
		{
			int hullIndex = Result.IndexOf('H');
			bool hasHull = hullIndex >= 0;
			int riggingIndex = Result.IndexOf('R');
			bool hasRigging = riggingIndex >= 0;
			Marines = Result.Contains<char>('M') ? 1 : 0;
			ChanceOfFire = Result.Contains<char>('*');
			if (hasHull && hasRigging)
			{
				int smallest = Math.Min(hullIndex, riggingIndex);
				if (smallest == hullIndex)
				{
					ParseHullDamage(hullIndex, riggingIndex);
				}
				else
				{
					ParseRiggingDamage(riggingIndex, hullIndex);
				}
			}
			else if (hasHull)
			{
				ParseHullDamage(hullIndex, riggingIndex);
			}
			else if (hasRigging)
			{
				ParseRiggingDamage(riggingIndex, hullIndex);
			}
			else
			{
				Hull = 0;
				Rigging = 0;
			}
		}

		private void ParseHullDamage(int hullIndex, int riggingIndex)
		{
			if (hullIndex == 0)
			{
				Hull = 1;
				if (riggingIndex == 1)
				{
					Rigging = 1;
				}
				else if (riggingIndex == 2)
				{
					Rigging = Int32.Parse(Result[1].ToString());
				}
			}
			else
			{
				Hull = Int32.Parse(Result[0].ToString());
				if (riggingIndex == 2)
				{
					Rigging = 1;
				}
				else if (riggingIndex == 3)
				{
					Rigging = Int32.Parse(Result[2].ToString());
				}
			}
		}

		private void ParseRiggingDamage(int riggingIndex, int hullIndex)
		{
			if (riggingIndex == 0)
			{
				Rigging = 1;
				if (hullIndex == 1)
				{
					Hull = 1;
				}
				else if (hullIndex == 2)
				{
					Hull = Int32.Parse(Result[1].ToString());
				}
			}
			else
			{
				Rigging = Int32.Parse(Result[0].ToString());
				if (hullIndex == 2)
				{
					Hull = 1;
				}
				else if (hullIndex == 3)
				{
					Hull = Int32.Parse(Result[2].ToString());
				}
			}
		} 
		#endregion

		#region Object Overrides

		public override string ToString()
		{
			return Result;
		}

		#endregion
	}
}
