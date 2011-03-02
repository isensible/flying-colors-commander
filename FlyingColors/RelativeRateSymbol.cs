using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingColors
{
	public static class RelativeRateSymbol
	{
		public static readonly string NoRate = "-";
		public static readonly string Gunboat = "T";
		public static readonly string GunBrig = "G";
		public static readonly string SixthRate = "6";
		public static readonly string FifthRate = "5";
		public static readonly string FourthRate = "4";
		public static readonly string ThirdRate = "3";
		public static readonly string SecondRate = "2";
		public static readonly string FirstRate = "1";
		public static readonly string FirstRateHeavy = "(1)";

		public static readonly LinkedList<string> Symbols;

		static RelativeRateSymbol()
		{
			var list = new LinkedList<string>();
			var node = list.AddFirst(FirstRateHeavy);
			node = list.AddAfter(node, FirstRate);
			node = list.AddAfter(node, SecondRate);
			node = list.AddAfter(node, ThirdRate);
			node = list.AddAfter(node, FourthRate);
			node = list.AddAfter(node, FifthRate);
			node = list.AddAfter(node, SixthRate);
			node = list.AddAfter(node, GunBrig);
			node = list.AddAfter(node, Gunboat);
			node = list.AddAfter(node, NoRate);
			Symbols = list;
		}

		public static string ShiftUp(string symbol)
		{
			var symbolNode = Symbols.Find(symbol);
			if (symbolNode == null)
			{
				return symbol;
			}
			if (symbolNode.Next == null)
			{
				return symbol;
			}
			return symbolNode.Next.Value;
		}

		public static string ShiftUp(string symbol, int shiftCount)
		{
			string newSymbol = string.Copy(symbol);
			for (int shifts = 0; shifts < shiftCount; ++shiftCount)
			{
				newSymbol = ShiftUp(newSymbol);
				if (newSymbol == symbol || newSymbol == NoRate)
				{
					return newSymbol;
				}
			}
			return newSymbol;
		}

		public static string ShiftDown(string symbol)
		{
			var symbolNode = Symbols.Find(symbol);
			if (symbolNode == null)
			{
				return symbol;
			}
			if (symbolNode.Previous == null)
			{
				return symbol;
			}
			return symbolNode.Previous.Value;
		}

		public static string ShiftDown(string symbol, int shiftCount)
		{
			string newSymbol = string.Copy(symbol);
			for (int shifts = 0; shifts < shiftCount; ++shiftCount)
			{
				newSymbol = ShiftDown(newSymbol);
				if (newSymbol == symbol || newSymbol == NoRate)
				{
					return newSymbol;
				}
			}
			return newSymbol;
		}
	}
}
