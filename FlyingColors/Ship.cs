using System;
using Castle.ActiveRecord;
using Csla;
using FlyingColors.DataModel;

namespace FlyingColors
{
	[Serializable]
	public class Ship : BusinessBase<Ship>
	{
		private ShipData _ship = null;

		public static readonly PropertyInfo<int> ShipIdProperty = RegisterProperty<int>(c => c.ShipId);
		/// <Summary>
		/// Gets or sets the ShipId value.
		/// </Summary>
		public int ShipId
		{
			get { return GetProperty(ShipIdProperty); }
			set { SetProperty(ShipIdProperty, value); }
		}

		public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
		/// <Summary>
		/// Gets or sets the Name value.
		/// </Summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}

		public static Ship NewShip(string name)
		{
			return DataPortal.Create<Ship>(name);
		}

		private void DataPortal_Create(string name)
		{
			_ship = new ShipData();
			LoadProperty<string>(NameProperty, name);
		}

		protected override void DataPortal_Insert()
		{
			ActiveRecordMediator.Create(ToData());
		}

		protected override void DataPortal_Update()
		{
			ActiveRecordMediator<ShipData>.Update(ToData());
		}

		protected override void DataPortal_DeleteSelf()
		{
			ActiveRecordMediator<ShipData>.Delete(ToData());
		}

		internal ShipData ToData()
		{
			_ship.Name = ReadProperty<string>(NameProperty);
			return _ship;
		}
	}	
}
