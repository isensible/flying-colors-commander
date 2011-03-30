using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingColors
{
	public class FirePower
	{
		public static readonly int NoEffect = -100;
		public static readonly bool Shaded = true;

		public int Value { get; private set; }
		public bool IsShaded { get; private set; }
		public bool HasNoEffect { get; private set; }

		/// <summary>
		/// Lookup of rate symbols against FirePower objects sorted by range.
		/// </summary>
		private static Dictionary<string, SortedList<int, FirePower>> firePowerTable = null;

		static FirePower()
		{
			firePowerTable = new Dictionary<string, SortedList<int, FirePower>>(10);
			var noRate = new SortedList<int, FirePower>(11);
			noRate.Add(0, new FirePower(NoEffect));
			noRate.Add(1, new FirePower(NoEffect));
			noRate.Add(2, new FirePower(NoEffect));
			noRate.Add(3, new FirePower(NoEffect));
			noRate.Add(4, new FirePower(NoEffect));
			noRate.Add(5, new FirePower(NoEffect));
			noRate.Add(6, new FirePower(NoEffect));
			noRate.Add(7, new FirePower(NoEffect));
			noRate.Add(8, new FirePower(NoEffect));
			noRate.Add(9, new FirePower(NoEffect));
			noRate.Add(10, new FirePower(NoEffect));
			firePowerTable.Add(RelativeRateSymbol.NoRate, noRate);
			var gunboat = new SortedList<int, FirePower>(11);
			gunboat.Add(0, new FirePower(2, Shaded));
			gunboat.Add(1, new FirePower(2, Shaded));
			gunboat.Add(2, new FirePower(1));
			gunboat.Add(3, new FirePower(1));
			gunboat.Add(4, new FirePower(0));
			gunboat.Add(5, new FirePower(0));
			gunboat.Add(6, new FirePower(NoEffect));
			gunboat.Add(7, new FirePower(NoEffect));
			gunboat.Add(8, new FirePower(NoEffect));
			gunboat.Add(9, new FirePower(NoEffect));
			gunboat.Add(10, new FirePower(NoEffect));
			firePowerTable.Add(RelativeRateSymbol.Gunboat, gunboat);
			var gunBrig = new SortedList<int, FirePower>(11);
			gunBrig.Add(0, new FirePower(3, Shaded));
			gunBrig.Add(1, new FirePower(3, Shaded));
			gunBrig.Add(2, new FirePower(2));
			gunBrig.Add(3, new FirePower(2));
			gunBrig.Add(4, new FirePower(1));
			gunBrig.Add(5, new FirePower(0));
			gunBrig.Add(6, new FirePower(NoEffect));
			gunBrig.Add(7, new FirePower(NoEffect));
			gunBrig.Add(8, new FirePower(NoEffect));
			gunBrig.Add(9, new FirePower(NoEffect));
			gunBrig.Add(10, new FirePower(NoEffect));
			firePowerTable.Add(RelativeRateSymbol.GunBrig, gunBrig);
			var rate6 = new SortedList<int, FirePower>(11);
			rate6.Add(0, new FirePower(4, Shaded));
			rate6.Add(1, new FirePower(4, Shaded));
			rate6.Add(2, new FirePower(3, Shaded));
			rate6.Add(3, new FirePower(3));
			rate6.Add(4, new FirePower(2));
			rate6.Add(5, new FirePower(1));
			rate6.Add(6, new FirePower(1));
			rate6.Add(7, new FirePower(0));
			rate6.Add(8, new FirePower(NoEffect));
			rate6.Add(9, new FirePower(NoEffect));
			rate6.Add(10, new FirePower(NoEffect));
			firePowerTable.Add(RelativeRateSymbol.SixthRate, rate6);
			var rate5 = new SortedList<int, FirePower>(11);
			rate5.Add(0, new FirePower(7, Shaded));
			rate5.Add(1, new FirePower(7, Shaded));
			rate5.Add(2, new FirePower(5, Shaded));
			rate5.Add(3, new FirePower(4, Shaded));
			rate5.Add(4, new FirePower(3));
			rate5.Add(5, new FirePower(2));
			rate5.Add(6, new FirePower(2));
			rate5.Add(7, new FirePower(1));
			rate5.Add(8, new FirePower(0));
			rate5.Add(9, new FirePower(NoEffect));
			rate5.Add(10, new FirePower(NoEffect));
			firePowerTable.Add(RelativeRateSymbol.FifthRate, rate5);
			var rate4 = new SortedList<int, FirePower>(11);
			rate4.Add(0, new FirePower(10, Shaded));
			rate4.Add(1, new FirePower(10, Shaded));
			rate4.Add(2, new FirePower(7, Shaded));
			rate4.Add(3, new FirePower(6, Shaded));
			rate4.Add(4, new FirePower(5));
			rate4.Add(5, new FirePower(4));
			rate4.Add(6, new FirePower(3));
			rate4.Add(7, new FirePower(2));
			rate4.Add(8, new FirePower(1));
			rate4.Add(9, new FirePower(0));
			rate4.Add(10, new FirePower(NoEffect));
			firePowerTable.Add(RelativeRateSymbol.FourthRate, rate4);
			var rate3 = new SortedList<int, FirePower>(11);
			rate3.Add(0, new FirePower(12, Shaded));
			rate3.Add(1, new FirePower(12, Shaded));
			rate3.Add(2, new FirePower(9, Shaded));
			rate3.Add(3, new FirePower(8, Shaded));
			rate3.Add(4, new FirePower(6));
			rate3.Add(5, new FirePower(5));
			rate3.Add(6, new FirePower(4));
			rate3.Add(7, new FirePower(2));
			rate3.Add(8, new FirePower(2));
			rate3.Add(9, new FirePower(1));
			rate3.Add(10, new FirePower(0));
			firePowerTable.Add(RelativeRateSymbol.ThirdRate, rate3);
			var rate2 = new SortedList<int, FirePower>(11);
			rate2.Add(0, new FirePower(15, Shaded));
			rate2.Add(1, new FirePower(15, Shaded));
			rate2.Add(2, new FirePower(12, Shaded));
			rate2.Add(3, new FirePower(10, Shaded));
			rate2.Add(4, new FirePower(7, Shaded));
			rate2.Add(5, new FirePower(6));
			rate2.Add(6, new FirePower(5));
			rate2.Add(7, new FirePower(3));
			rate2.Add(8, new FirePower(2));
			rate2.Add(9, new FirePower(2));
			rate2.Add(10, new FirePower(1));
			firePowerTable.Add(RelativeRateSymbol.SecondRate, rate2);
			var rate1 = new SortedList<int, FirePower>(11);
			rate1.Add(0, new FirePower(17, Shaded));
			rate1.Add(1, new FirePower(17, Shaded));
			rate1.Add(2, new FirePower(14, Shaded));
			rate1.Add(3, new FirePower(12, Shaded));
			rate1.Add(4, new FirePower(9, Shaded));
			rate1.Add(5, new FirePower(8, Shaded));
			rate1.Add(6, new FirePower(7));
			rate1.Add(7, new FirePower(5));
			rate1.Add(8, new FirePower(3));
			rate1.Add(9, new FirePower(2));
			rate1.Add(10, new FirePower(2));
			firePowerTable.Add(RelativeRateSymbol.FirstRate, rate1);
			var rate1Heavy = new SortedList<int, FirePower>(11);
			rate1Heavy.Add(0, new FirePower(19, Shaded));
			rate1Heavy.Add(1, new FirePower(19, Shaded));
			rate1Heavy.Add(2, new FirePower(16, Shaded));
			rate1Heavy.Add(3, new FirePower(14, Shaded));
			rate1Heavy.Add(4, new FirePower(11, Shaded));
			rate1Heavy.Add(5, new FirePower(10, Shaded));
			rate1Heavy.Add(6, new FirePower(8));
			rate1Heavy.Add(7, new FirePower(6));
			rate1Heavy.Add(8, new FirePower(4));
			rate1Heavy.Add(9, new FirePower(3));
			rate1Heavy.Add(10, new FirePower(2));
			firePowerTable.Add(RelativeRateSymbol.GunBrig, rate1Heavy);
		}

		private FirePower(int value, bool isShaded)
		{
			Value = value;
			IsShaded = isShaded;
			HasNoEffect = (value != NoEffect);
		}

		private FirePower(int value)
			: this(value, false)
		{
		}

		public static FirePower GetBase(RelativeRate rate, int range)
		{
			return firePowerTable[rate.Symbol][range];
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		internal FirePower Increment()
		{
			return new FirePower(this.Value++, this.IsShaded);
		}

		internal FirePower CopySelf()
		{
			return new FirePower(this.Value, this.IsShaded);
		}

		internal FirePower Decrement()
		{
			return new FirePower(this.Value--, this.IsShaded);
		}

		internal FirePower IncrementBy(int p)
		{
			var fp = CopySelf();
			for (int i = 0; i < p; ++i)
			{
				fp = Increment();
			}
			return fp;
		}
	}
}
