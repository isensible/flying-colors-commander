using System;
using Castle.ActiveRecord;
using Csla;
using FlyingColors.DataModel;

namespace FlyingColors
{
	/// <summary>
	/// A single Ship.
	/// </summary>
	[Serializable]
	public class Ship : BusinessBase<Ship>
	{
		#region Id and Name
		public static readonly PropertyInfo<long> ShipIdProperty = RegisterProperty<long>(c => c.ShipId);
		/// <Summary>
		/// Gets or sets the ShipId value.
		/// </Summary>
		public long ShipId
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
		#endregion

		#region Nationality and Victory Points

		public static readonly PropertyInfo<Nationality> NationalityProperty = RegisterProperty<Nationality>(c => c.Nationality);
		/// <Summary>
		/// Gets or sets the Nationality value.
		/// </Summary>
		public Nationality Nationality
		{
			get { return GetProperty(NationalityProperty); }
			set { SetProperty(NationalityProperty, value); }
		}

		public static readonly PropertyInfo<int> VictoryPointsProperty = RegisterProperty<int>(c => c.VictoryPoints);
		/// <Summary>
		/// Gets or sets the VictoryPoints value.
		/// </Summary>
		public int VictoryPoints
		{
			get { return GetProperty(VictoryPointsProperty); }
			set { SetProperty(VictoryPointsProperty, value); }
		}

		#endregion

		#region Relative Rate
		public static readonly PropertyInfo<RelativeRate> RateProperty = RegisterProperty<RelativeRate>(c => c.Rate);
		/// <Summary>
		/// Gets or sets the Rate value.
		/// </Summary>
		public RelativeRate Rate
		{
			get { return GetProperty(RateProperty); }
			set { SetProperty(RateProperty, value); }
		}

		public static readonly PropertyInfo<RelativeRate> RateDamagedProperty = RegisterProperty<RelativeRate>(c => c.RateDamaged);
		/// <Summary>
		/// Gets or sets the RateDamaged value.
		/// </Summary>
		public RelativeRate RateDamaged
		{
			get { return GetProperty(RateDamagedProperty); }
			set { SetProperty(RateDamagedProperty, value); }
		} 
		#endregion

		#region Damage Capacity

		public static readonly PropertyInfo<int> DamageCapacityProperty = RegisterProperty<int>(c => c.DamageCapacity);
		/// <Summary>
		/// Gets or sets the DamageCapacity value.
		/// </Summary>
		public int DamageCapacity
		{
			get { return GetProperty(DamageCapacityProperty); }
			set { SetProperty(DamageCapacityProperty, value); }
		}

		public static readonly PropertyInfo<int> DamageCapacityDamagedProperty = RegisterProperty<int>(c => c.DamageCapacityDamaged);
		/// <Summary>
		/// Gets or sets the DamageCapacityDamaged value.
		/// </Summary>
		public int DamageCapacityDamaged
		{
			get { return GetProperty(DamageCapacityDamagedProperty); }
			set { SetProperty(DamageCapacityDamagedProperty, value); }
		}

		#endregion

		#region Marines
		public static readonly PropertyInfo<int> MarinesProperty = RegisterProperty<int>(c => c.Marines);
		/// <Summary>
		/// Gets or sets the Marines value.
		/// </Summary>
		public int Marines
		{
			get { return GetProperty(MarinesProperty); }
			set { SetProperty(MarinesProperty, value); }
		}

		public static readonly PropertyInfo<int> MarinesDamagedProperty = RegisterProperty<int>(c => c.MarinesDamaged);
		/// <Summary>
		/// Gets or sets the MarinesDamaged value.
		/// </Summary>
		public int MarinesDamaged
		{
			get { return GetProperty(MarinesDamagedProperty); }
			set { SetProperty(MarinesDamagedProperty, value); }
		} 
		#endregion

		#region Factory Methods
		public static Ship NewShip(string name)
		{
			return DataPortal.Create<Ship>(name);
		}

		internal static Ship GetShip(ShipData shipData)
		{
			return DataPortal.FetchChild<Ship>(shipData);
		}

		private Ship()
		{
		}
		#endregion

		#region Data Portal
		private ShipData _ship = null;

		private void DataPortal_Create(string name)
		{
			_ship = new ShipData();
			LoadProperty<string>(NameProperty, name);
			LoadProperty<Nationality>(NationalityProperty, Nationality.British);
			LoadProperty<int>(VictoryPointsProperty, 5);
			LoadProperty<RelativeRate>(RateProperty, RelativeRate.NoRate);
			LoadProperty<RelativeRate>(RateDamagedProperty, RelativeRate.NoRate);
			LoadProperty<int>(DamageCapacityProperty, 13);
			LoadProperty<int>(DamageCapacityDamagedProperty, 11);
			LoadProperty<int>(MarinesProperty, 4);
			LoadProperty<int>(MarinesDamagedProperty, 3);
		}

		private void Child_Fetch(ShipData shipData)
		{
			_ship = shipData;
			LoadProperty<long>(ShipIdProperty, _ship.ShipId);
			LoadProperty<string>(NameProperty, _ship.Name);
			LoadProperty<Nationality>(NationalityProperty, (Nationality)Enum.Parse(typeof(Nationality), _ship.Nationality));
			LoadProperty<int>(VictoryPointsProperty, _ship.VictoryPoints);
			LoadProperty<RelativeRate>(RateProperty, RelativeRate.NewRate(
				_ship.Rate,
				(RelativeRateColor)Enum.Parse(typeof(RelativeRateColor), _ship.RateColor),
				(RelativeRateShape)Enum.Parse(typeof(RelativeRateShape), _ship.RateShape)));
			LoadProperty<RelativeRate>(RateDamagedProperty, RelativeRate.NewRate(
				_ship.RateDamaged,
				(RelativeRateColor)Enum.Parse(typeof(RelativeRateColor), _ship.RateColorDamaged),
				(RelativeRateShape)Enum.Parse(typeof(RelativeRateShape), _ship.RateShapeDamaged)));
			LoadProperty<int>(DamageCapacityProperty, _ship.DamageCapacity);
			LoadProperty<int>(DamageCapacityDamagedProperty, _ship.DamageCapacityDamaged);
			LoadProperty<int>(MarinesProperty, _ship.Marines);
			LoadProperty<int>(MarinesDamagedProperty, _ship.MarinesDamaged);
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
			_ship.Nationality = ReadProperty<Nationality>(NationalityProperty).ToString();
			_ship.VictoryPoints = ReadProperty<int>(VictoryPointsProperty);
			_ship.Rate = ReadProperty<RelativeRate>(RateProperty).Symbol;
			_ship.RateColor = ReadProperty<RelativeRate>(RateProperty).Color.ToString();
			_ship.RateShape = ReadProperty<RelativeRate>(RateProperty).Shape.ToString();
			_ship.RateDamaged = ReadProperty<RelativeRate>(RateDamagedProperty).Symbol;
			_ship.RateColorDamaged = ReadProperty<RelativeRate>(RateDamagedProperty).Color.ToString();
			_ship.RateShapeDamaged = ReadProperty<RelativeRate>(RateDamagedProperty).Shape.ToString();
			_ship.DamageCapacity = ReadProperty<int>(DamageCapacityProperty);
			_ship.DamageCapacityDamaged = ReadProperty<int>(DamageCapacityDamagedProperty);
			_ship.Marines = ReadProperty<int>(MarinesProperty);
			_ship.MarinesDamaged = ReadProperty<int>(MarinesDamagedProperty);
			return _ship;
		} 
		#endregion		
	}	
}
