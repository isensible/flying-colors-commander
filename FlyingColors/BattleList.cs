using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using Castle.ActiveRecord;
using FlyingColors.DataModel;

namespace FlyingColors
{
	[Serializable]
	public class BattleList : DynamicListBase<Battle>
	{
		public static BattleList GetBattles()
		{
			return DataPortal.Fetch<BattleList>();
		}

		private void DataPortal_Fetch()
		{
			RaiseListChangedEvents = false;
			var battles = ActiveRecordMediator<BattleData>.FindAll();
			foreach (var battle in battles)
			{
				Add(Battle.GetBattle(battle));
			}
			RaiseListChangedEvents = true;
		}
	}
}
