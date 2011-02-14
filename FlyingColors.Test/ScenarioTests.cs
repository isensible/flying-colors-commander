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
		[UseSqlite]
		public void create_cape_ortugal_scenario()
		{
			Ship ceasar = Ship.NewShip("Ceasar");
			ceasar = ceasar.Save();
			Ship hero = Ship.NewShip("Hero");
			hero = hero.Save();
			Ship courageux = Ship.NewShip("Courageux");
			courageux = courageux.Save();

			Ship dugay = Ship.NewShip("Dugay-Trouin");
			dugay = dugay.Save();
			Ship formidable = Ship.NewShip("Formidable");
			formidable = formidable.Save();
			Ship montBlanc = Ship.NewShip("Mont-Blanc");
			montBlanc = montBlanc.Save();
			Ship scipion = Ship.NewShip("Scipion");
			scipion = scipion.Save();

			Scenario capeOrtugal = Scenario.NewScenario();
			capeOrtugal.Name = "Cape Ortugal";
			capeOrtugal.Year = 1805;
			capeOrtugal.WindDirection = 6;
			capeOrtugal.WeatherEffects = Weather.Normal;
			capeOrtugal.Turns = 12;

			Fleet british = Fleet.NewFleet();
			british.Team = "British";
			british.Nationality = "British";
			british.Audacity = 3;
			british.Ships.Add(FleetShip.NewFleetShip(ceasar));
			british.Ships.Add(FleetShip.NewFleetShip(hero));
			british.Ships.Add(FleetShip.NewFleetShip(courageux));
			capeOrtugal.Fleets.Add(british);

			Fleet french = Fleet.NewFleet();
			french.Team = "French";
			french.Nationality = "French";
			french.Audacity = 1;
			french.Ships.Add(FleetShip.NewFleetShip(dugay));
			french.Ships.Add(FleetShip.NewFleetShip(formidable));
			french.Ships.Add(FleetShip.NewFleetShip(montBlanc));
			french.Ships.Add(FleetShip.NewFleetShip(scipion));
			capeOrtugal.Fleets.Add(french);

			capeOrtugal = capeOrtugal.Save();			
		}
	}
}
