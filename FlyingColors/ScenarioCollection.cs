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

		public static ScenarioCollection NewScenariosCollection()
		{
			return DataPortal.Create<ScenarioCollection>();
		}

		public static ScenarioCollection GetScenariosCollection()
		{
			return DataPortal.Fetch<ScenarioCollection>();
		}

		#endregion

		#region Data Access

		private string database = "scenarios";

		private void DataPortal_Create()
		{
			if (PersistentDictionaryFile.Exists(database))
			{
				PersistentDictionaryFile.DeleteFiles(database);
			}			
		}

		private void DataPortal_Fetch()
		{
			using (var db = new PersistentDictionary<string, ScenarioData>(database))
			{
				foreach (var key in db.Keys)
				{
					Add(Scenario.GetScenario(db[key]));
				}
			}
		}

		#endregion
	}
}
