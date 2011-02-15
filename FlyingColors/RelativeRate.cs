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
		public static readonly PropertyInfo<char> SymbolProperty = RegisterProperty<char>(c => c.Symbol);
		/// <summary>
		/// Gets the Symbol value.
		/// </summary>
		public char Symbol
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
		public static RelativeRate NewRate(char symbol, RelativeRateColor color, RelativeRateShape shape)
		{
			return DataPortal.CreateChild<RelativeRate>(
				new Tuple<char, RelativeRateColor, RelativeRateShape>(symbol, color, shape));
		}

		public static RelativeRate FirstRate
		{
			get { return NewRate(RelativeRateSymbol.FirstRate, RelativeRateColor.NoColor, RelativeRateShape.NoShape); }
		}

		public static RelativeRate SecondRate
		{
			get { return NewRate(RelativeRateSymbol.SecondRate, RelativeRateColor.NoColor, RelativeRateShape.NoShape); }
		}

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
		private void Child_Create(Tuple<char, RelativeRateColor, RelativeRateShape> tuple)
		{
			Symbol = tuple.Item1;
			Color = tuple.Item2;
			Shape = tuple.Item3;
		}		 
		#endregion
	}
}
