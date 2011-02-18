using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;

namespace FlyingColors
{
	/// <summary>
	/// Immutable rate of a Ship used for calculating fire power.
	/// </summary>
	[Serializable]
	public class RelativeRate : ReadOnlyBase<RelativeRate>
	{
		#region Symbol
		public static readonly PropertyInfo<string> SymbolProperty = RegisterProperty<string>(c => c.Symbol);
		/// <summary>
		/// Gets the Symbol value.
		/// </summary>
		public string Symbol
		{
			get { return GetProperty(SymbolProperty); }
			private set { LoadProperty(SymbolProperty, value); }
		}
		#endregion

		#region Color
		public static readonly PropertyInfo<RelativeRateColor> ColorProperty = RegisterProperty<RelativeRateColor>(c => c.Color);
		/// <Summary>
		/// Gets the Color value.
		/// </Summary>
		public RelativeRateColor Color
		{
			get { return GetProperty(ColorProperty); }
			private set { LoadProperty(ColorProperty, value); }
		}
		#endregion

		#region Shape
		public static readonly PropertyInfo<RelativeRateShape> ShapeProperty = RegisterProperty<RelativeRateShape>(c => c.Shape);
		/// <summary>
		/// Gets the Shape value.
		/// </summary>
		public RelativeRateShape Shape
		{
			get { return GetProperty(ShapeProperty); }
			private set { LoadProperty(ShapeProperty, value); }
		}
		#endregion

		#region Factory Methods
		/// <summary>
		/// Returns a new Relative Rate.
		/// </summary>
		public static RelativeRate NewRate(string symbol, RelativeRateColor color, RelativeRateShape shape)
		{
			return DataPortal.CreateChild<RelativeRate>(
				new Tuple<string, RelativeRateColor, RelativeRateShape>(symbol, color, shape));
		}

		#region First Rate
		public static RelativeRate FirstRate
		{
			get { return NewRate(RelativeRateSymbol.FirstRate, RelativeRateColor.NoColor, RelativeRateShape.NoShape); }
		}

		public static RelativeRate FirstRateBlackCircle
		{
			get { return NewRate(RelativeRateSymbol.FirstRate, RelativeRateColor.Black, RelativeRateShape.Circle); }
		}

		public static RelativeRate FirstRateWhiteCircle
		{
			get { return NewRate(RelativeRateSymbol.FirstRate, RelativeRateColor.White, RelativeRateShape.Circle); }
		}

		public static RelativeRate FirstRateYellowCircle
		{
			get { return NewRate(RelativeRateSymbol.FirstRate, RelativeRateColor.Yellow, RelativeRateShape.Circle); }
		}

		public static RelativeRate FirstRateRedHexagon
		{
			get { return NewRate(RelativeRateSymbol.FirstRate, RelativeRateColor.Red, RelativeRateShape.Hexagon); }
		}
		#endregion

		#region Second Rate
		public static RelativeRate SecondRate
		{
			get { return NewRate(RelativeRateSymbol.SecondRate, RelativeRateColor.NoColor, RelativeRateShape.NoShape); }
		}

		public static RelativeRate SecondRateBlackCircle
		{
			get { return NewRate(RelativeRateSymbol.SecondRate, RelativeRateColor.Black, RelativeRateShape.Circle); }
		}

		public static RelativeRate SecondRateWhiteCircle
		{
			get { return NewRate(RelativeRateSymbol.SecondRate, RelativeRateColor.White, RelativeRateShape.Circle); }
		}

		public static RelativeRate SecondRateYellowCircle
		{
			get { return NewRate(RelativeRateSymbol.SecondRate, RelativeRateColor.Yellow, RelativeRateShape.Circle); }
		}

		public static RelativeRate SecondRateRedHexagon
		{
			get { return NewRate(RelativeRateSymbol.SecondRate, RelativeRateColor.Red, RelativeRateShape.Hexagon); }
		}
		#endregion

		#region Third Rate
		public static RelativeRate ThirdRate
		{
			get { return NewRate(RelativeRateSymbol.ThirdRate, RelativeRateColor.NoColor, RelativeRateShape.NoShape); }
		}

		public static RelativeRate ThirdRateBlackCircle
		{
			get { return NewRate(RelativeRateSymbol.ThirdRate, RelativeRateColor.Black, RelativeRateShape.Circle); }
		}

		public static RelativeRate ThirdRateWhiteCircle
		{
			get { return NewRate(RelativeRateSymbol.ThirdRate, RelativeRateColor.White, RelativeRateShape.Circle); }
		}

		public static RelativeRate ThirdRateYellowCircle
		{
			get { return NewRate(RelativeRateSymbol.ThirdRate, RelativeRateColor.Yellow, RelativeRateShape.Circle); }
		}

		public static RelativeRate ThirdRateRedHexagon
		{
			get { return NewRate(RelativeRateSymbol.ThirdRate, RelativeRateColor.Red, RelativeRateShape.Hexagon); }
		}
		#endregion

		#region Fourth Rate
		public static RelativeRate FourthRate
		{
			get { return NewRate(RelativeRateSymbol.FourthRate, RelativeRateColor.NoColor, RelativeRateShape.NoShape); }
		}

		public static RelativeRate FourthRateBlackCircle
		{
			get { return NewRate(RelativeRateSymbol.FourthRate, RelativeRateColor.Black, RelativeRateShape.Circle); }
		}

		public static RelativeRate FourthRateWhiteCircle
		{
			get { return NewRate(RelativeRateSymbol.FourthRate, RelativeRateColor.White, RelativeRateShape.Circle); }
		}

		public static RelativeRate FourthRateYellowCircle
		{
			get { return NewRate(RelativeRateSymbol.FourthRate, RelativeRateColor.Yellow, RelativeRateShape.Circle); }
		}

		public static RelativeRate FourthRateRedHexagon
		{
			get { return NewRate(RelativeRateSymbol.FourthRate, RelativeRateColor.Red, RelativeRateShape.Hexagon); }
		}
		#endregion

		#region Fifth Rate
		public static RelativeRate FifthRate
		{
			get { return NewRate(RelativeRateSymbol.FifthRate, RelativeRateColor.NoColor, RelativeRateShape.NoShape); }
		}

		public static RelativeRate FifthRateBlackCircle
		{
			get { return NewRate(RelativeRateSymbol.FifthRate, RelativeRateColor.Black, RelativeRateShape.Circle); }
		}

		public static RelativeRate FifthRateWhiteCircle
		{
			get { return NewRate(RelativeRateSymbol.FifthRate, RelativeRateColor.White, RelativeRateShape.Circle); }
		}

		public static RelativeRate FifthRateYellowCircle
		{
			get { return NewRate(RelativeRateSymbol.FifthRate, RelativeRateColor.Yellow, RelativeRateShape.Circle); }
		}

		public static RelativeRate FifthRateRedHexagon
		{
			get { return NewRate(RelativeRateSymbol.FifthRate, RelativeRateColor.Red, RelativeRateShape.Hexagon); }
		}
		#endregion

		#region Sixth Rate
		public static RelativeRate SixthRate
		{
			get { return NewRate(RelativeRateSymbol.SixthRate, RelativeRateColor.NoColor, RelativeRateShape.NoShape); }
		}

		public static RelativeRate SixthRateBlackCircle
		{
			get { return NewRate(RelativeRateSymbol.SixthRate, RelativeRateColor.Black, RelativeRateShape.Circle); }
		}

		public static RelativeRate SixthRateWhiteCircle
		{
			get { return NewRate(RelativeRateSymbol.SixthRate, RelativeRateColor.White, RelativeRateShape.Circle); }
		}

		public static RelativeRate SixthRateYellowCircle
		{
			get { return NewRate(RelativeRateSymbol.SixthRate, RelativeRateColor.Yellow, RelativeRateShape.Circle); }
		}

		public static RelativeRate SixthRateRedHexagon
		{
			get { return NewRate(RelativeRateSymbol.SixthRate, RelativeRateColor.Red, RelativeRateShape.Hexagon); }
		}
		#endregion

		/// <summary>
		/// Relative Rate for Ship that cannot fire.
		/// </summary>
		public static RelativeRate NoRate
		{
			get { return NewRate(RelativeRateSymbol.NoRate, RelativeRateColor.NoColor, RelativeRateShape.NoShape); }
		}

		private RelativeRate()
		{
		}
		#endregion

		#region Data Portal
		private void Child_Create(Tuple<string, RelativeRateColor, RelativeRateShape> tuple)
		{
			Symbol = tuple.Item1;
			Color = tuple.Item2;
			Shape = tuple.Item3;
		}
		#endregion
	}
}
