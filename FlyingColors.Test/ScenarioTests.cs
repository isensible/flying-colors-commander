using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace FlyingColors.Test
{
	public class ScenarioTests
	{
		[Fact]
		public void create_cape_ortugal_scenario()
		{
			var scenarios = ScenarioCollection.NewScenariosCollection();
			var capeOrtugal = Scenario.NewScenario().SetName("Cape Ortugal").SetYear(1805);
			scenarios.Add(capeOrtugal);

			Fleet britishFleet = Fleet.NewFleet().SetName("British").SetAudacity(3);
			capeOrtugal.AddFleet(britishFleet);
			Fleet frenchFleet = Fleet.NewFleet().SetName("French").SetAudacity(1);
			capeOrtugal.AddFleet(frenchFleet);

			Ship ceasar = Ship.NewShip()
				.SetName("Ceasar")
				.SetRate(2)
				.SetDamagedRate(4)
				.SetHull(14)
				.SetDamagedHull(13)
				.SetMarines(3)
				.SetFlagship(true);

			Commander strachan = Commander.NewCommander().SetName("Strachan").SetQuality(2).SetRange(4);
			ceasar.AddCommander(strachan);

			britishFleet.AddShip(ceasar);			
		}
	}
}
