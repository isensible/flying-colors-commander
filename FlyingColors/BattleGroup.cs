using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using FlyingColors.DataModel;
using Castle.ActiveRecord;

namespace FlyingColors
{
	[Serializable]
	public class BattleGroup : BusinessBase<BattleGroup>
	{
		public static PropertyInfo<long> BattleGroupIdProperty = RegisterProperty<long>(c => c.BattleGroupId);
		public long BattleGroupId
		{
			get { return GetProperty(BattleGroupIdProperty); }
			private set { LoadProperty(BattleGroupIdProperty, value); }
		}	

		public static PropertyInfo<Nationality> NationalityProperty = RegisterProperty<Nationality>(c => c.Nationality);
		public Nationality Nationality
		{
			get { return GetProperty(NationalityProperty); }
			private set { LoadProperty(NationalityProperty, value); }
		}

		public static PropertyInfo<BattleShipList> ShipsProperty = RegisterProperty<BattleShipList>(c => c.Ships);
		public BattleShipList Ships
		{
			get { return GetProperty(ShipsProperty); }
			set { SetProperty(ShipsProperty, value); }
		}

		/// <summary>
		/// Creates a new BattleGroup from a grouping of fleets.
		/// </summary>
		/// <param name="fleets">A grouping of fleets keyed by thier Battle Group Nationality.</param>
		/// <returns>The new BattleGroup populated with Battle Ships from each grouped fleet.</returns>
		internal static BattleGroup NewBattleGroup(IGrouping<string, FleetData> fleets)
		{
			return DataPortal.CreateChild<BattleGroup>(fleets);
		}

		#region Data Portal

		private BattleGroupData _group = null;

		private void Child_Create(IGrouping<string, FleetData> fleets)
		{
			_group = new BattleGroupData();
			LoadProperty<Nationality>(NationalityProperty, (Nationality)Enum.Parse(typeof(Nationality), fleets.Key));
			LoadProperty<BattleShipList>(ShipsProperty, BattleShipList.New(fleets));				
		}

		private void Child_Insert(BattleData battle)
		{
			ToData(battle);
			FieldManager.UpdateChildren(_group);
			ActiveRecordMediator<BattleGroupData>.Create(_group);
			LoadProperty<long>(BattleGroupIdProperty, _group.BattleGroupId);
		}

		private void Child_Update(BattleData battle)
		{
			ToData(battle);
			FieldManager.UpdateChildren(_group);
			ActiveRecordMediator<BattleGroupData>.Update(_group);
		}

		private void Child_Delete(BattleData battle)
		{
			ToData(battle);
			FieldManager.UpdateChildren(_group);
			ActiveRecordMediator<BattleGroupData>.Delete(_group);
		}

		internal BattleGroupData ToData(BattleData battle)
		{
			_group.Battle = battle;
			_group.Nationality = ReadProperty<Nationality>(NationalityProperty).ToString();
			_group.Ships = ReadProperty<BattleShipList>(ShipsProperty).ToData(_group);
			return _group;
		}

		#endregion
	}
}
