using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using FlyingColors.DataModel;
using Castle.ActiveRecord;
using System.Diagnostics;

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
		internal static BattleGroup NewBattleGroup(BattleData battle, IGrouping<string, FleetData> fleets)
		{
			return DataPortal.CreateChild<BattleGroup>(new Tuple<BattleData, IGrouping<string,FleetData>>(battle, fleets));
		}

		internal static BattleGroup GetBattleGroup(BattleData battle, IGrouping<string, FleetData> fleets)
		{
			return DataPortal.FetchChild<BattleGroup>(new Tuple<BattleData, IGrouping<string, FleetData>>(battle, fleets));
		}

		#region Data Portal

		private IList<BattleGroupData> _groups = null;
		private int _groupIndex = -1;
		private BattleGroupData _group = null;

		private void Child_Create(Tuple<BattleData, IGrouping<string, FleetData>> tuple)
		{
			_groups = tuple.Item1.BattleGroups;
			_group = new BattleGroupData();
			_group.Battle = tuple.Item1;
			_groups.Add(_group);
			_groupIndex = _groups.IndexOf(_group); 
			LoadProperty<Nationality>(NationalityProperty, (Nationality)Enum.Parse(typeof(Nationality), tuple.Item2.Key));
			LoadProperty<BattleShipList>(ShipsProperty, BattleShipList.New(_group, tuple.Item2));				
		}

		private void Child_Fetch(Tuple<BattleData, IGrouping<string, FleetData>> tuple)
		{			
			_groups = tuple.Item1.BattleGroups;
			_group = _groups.FirstOrDefault<BattleGroupData>(g => g.Nationality == tuple.Item2.Key);
			Debug.Assert(_group != null);
			_groupIndex = _groups.IndexOf(_group);
			LoadProperty<long>(BattleGroupIdProperty, _group.BattleGroupId);
			LoadProperty<Nationality>(NationalityProperty, (Nationality)Enum.Parse(typeof(Nationality), tuple.Item2.Key));
			LoadProperty<BattleShipList>(ShipsProperty, BattleShipList.GetShips(_group, tuple.Item2));							
		}

		private void Child_Insert(BattleData battle)
		{
			LoadProperty<long>(BattleGroupIdProperty, _group.BattleGroupId);
			FieldManager.UpdateChildren(_group);
		}

		private void Child_Update(BattleData battle)
		{
			FieldManager.UpdateChildren(_group);
		}

		private void Child_Delete(BattleData battle)
		{
			FieldManager.UpdateChildren(_group);
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
