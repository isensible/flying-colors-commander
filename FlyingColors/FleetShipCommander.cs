using System;
using Castle.ActiveRecord;
using Csla;
using FlyingColors.DataModel;

namespace FlyingColors
{
	/// <summary>
	/// A Commander on board a Fleet Ship.
	/// </summary>
	[Serializable]
	public class FleetShipCommander : BusinessBase<FleetShipCommander>
	{
		#region ID
		public static readonly PropertyInfo<long> FleetShipCommanderIdProperty = RegisterProperty<long>(c => c.FleetShipCommanderId);
		/// <Summary>
		/// Gets or sets the FleetShipCommanderId value.
		/// </Summary>
		public long FleetShipCommanderId
		{
			get { return GetProperty(FleetShipCommanderIdProperty); }
			set { SetProperty(FleetShipCommanderIdProperty, value); }
		} 
		#endregion

		#region Commander
		public static readonly PropertyInfo<Commander> CommanderProperty = RegisterProperty<Commander>(c => c.Commander);
		/// <Summary>
		/// Gets or sets the Commander value.
		/// </Summary>
		public Commander Commander
		{
			get { return GetProperty(CommanderProperty); }
			set { SetProperty(CommanderProperty, value); }
		} 
		#endregion

		#region Factory Methods
		public static FleetShipCommander NewFleetShipCommander(Commander commander)
		{
			return DataPortal.CreateChild<FleetShipCommander>(commander);
		}

		internal static FleetShipCommander GetFleetShipCommander(FleetShipCommanderData shipCommander)
		{
			return DataPortal.FetchChild<FleetShipCommander>(shipCommander);
		}

		private FleetShipCommander()
		{
		} 
		#endregion

		#region Data Portal
		private FleetShipCommanderData _shipCommander = null;

		private void Child_Create(Commander commander)
		{
			_shipCommander = new FleetShipCommanderData();
			LoadProperty<Commander>(CommanderProperty, commander);
		}

		private void Child_Fetch(FleetShipCommanderData shipCommander)
		{
			_shipCommander = shipCommander;
			LoadProperty<long>(FleetShipCommanderIdProperty, _shipCommander.FleetShipCommanderId);
			LoadProperty<Commander>(CommanderProperty, Commander.GetCommander(_shipCommander.Commander));
		}

		private void Child_Insert(FleetShipData fleetShipData)
		{
			ToData(fleetShipData);
			ActiveRecordMediator<FleetShipData>.Create(_shipCommander);
			LoadProperty<long>(FleetShipCommanderIdProperty, _shipCommander.FleetShipCommanderId);
		}

		private void Child_Update(FleetShipData fleetShipData)
		{
			ToData(fleetShipData);
			ActiveRecordMediator<FleetShipData>.Update(_shipCommander);
		}

		private void Child_Delete(FleetShipData fleetShipData)
		{
			ToData(fleetShipData);
			ActiveRecordMediator<FleetShipData>.Delete(_shipCommander);
		}

		internal FleetShipCommanderData ToData(FleetShipData fleetShipData)
		{
			_shipCommander.FleetShip = fleetShipData;
			_shipCommander.Commander = ReadProperty<Commander>(CommanderProperty).ToData();
			return _shipCommander;
		} 
		#endregion
	}
}
