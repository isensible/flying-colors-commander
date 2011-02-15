using System;
using Csla;
using Castle.ActiveRecord;
using FlyingColors.DataModel;

namespace FlyingColors
{
	[Serializable]
	public class CommanderList : DynamicListBase<Commander>
	{
		public static CommanderList GetAllCommanders()
		{
			return DataPortal.Fetch<CommanderList>();
		}

		private void DataPortal_Fetch()
		{
			RaiseListChangedEvents = false;
			var commanders = ActiveRecordMediator<CommanderData>.FindAll();
			foreach (var commander in commanders)
			{
				Add(Commander.GetCommander(commander));
			}
			RaiseListChangedEvents = true;
		}
	}
}
