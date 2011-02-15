using System;
using Csla;
using Castle.ActiveRecord;
using FlyingColors.DataModel;

namespace FlyingColors
{
	[Serializable]
	public class ScenarioList : DynamicListBase<Scenario>
	{
		public static ScenarioList GetScenarios()
		{
			return DataPortal.Fetch<ScenarioList>();
		}

		private void DataPortal_Fetch()
		{
			RaiseListChangedEvents = false;
			var scenarios = ActiveRecordMediator<ScenarioData>.FindAll();
			foreach (var scenario in scenarios)
			{
				Add(Scenario.GetScenario(scenario));
			}
			RaiseListChangedEvents = true;
		}
	}
}
