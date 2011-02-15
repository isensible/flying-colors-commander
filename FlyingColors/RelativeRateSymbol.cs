using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingColors
{
	public static class RelativeRateSymbol
	{
		public static readonly char[] Symbols = new char[9]
		{ '0', 'T', 'G', '6', '5', '4', '3', '2', '1' };

		public static readonly char NoRate = Symbols[0];
		public static readonly char Gunboat = Symbols[1];
		public static readonly char GunBrig = Symbols[2];
		public static readonly char SixthRate = Symbols[3];
		public static readonly char FifthRate = Symbols[4];
		public static readonly char FourthRate = Symbols[5];
		public static readonly char ThirdRate = Symbols[6];
		public static readonly char SecondRate = Symbols[7];
		public static readonly char FirstRate = Symbols[8];
	}
}
