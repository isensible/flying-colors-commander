using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;

namespace FlyingColors
{
	[Serializable]
    public class Ship : BusinessBase<Ship>
	{
		#region Business Methods

		public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
		/// <Summary>
		/// Gets or sets the Name value.
		/// </Summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}

		#endregion

		#region Factory Methods

		public static Ship NewShip(string name)
		{
			return DataPortal.Create<Ship>(name);
		}

		#endregion

		#region Data Access

		[RunLocal]
		private void DataPortal_Create(string name)
		{
			LoadProperty<string>(NameProperty, name);
		}

		#endregion
	}

	
}
