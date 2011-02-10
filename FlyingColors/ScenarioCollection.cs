using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;
using Microsoft.Isam.Esent.Collections.Generic;

namespace FlyingColors
{
	[Serializable]
	public class ScenarioCollection : BusinessListBase<ScenarioCollection, Scenario>
	{
		#region Factory Methods

		public static ScenarioCollection CreateScenarios()
		{
			return DataPortal.Create<ScenarioCollection>();
		}

		public static ScenarioCollection GetScenarios()
		{
			return DataPortal.Fetch<ScenarioCollection>();
		}

		#endregion

		#region Data Access

		private string databaseFileName = "scenarios";

		private void DataPortal_Create()
		{
			using (var db = new PersistentDictionary<string, string>(databaseFileName))
			{

			}
		}

		private void DataPortal_Fetch()
		{
			Add(Scenario.NewScenario("Cape Ortugal"));
		}

		#endregion
	}
}
