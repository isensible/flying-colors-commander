﻿using System;
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
			ceasar.Nationality = Nationality.British;
			ceasar.VictoryPoints = 7;
			ceasar.Rate = RelativeRate.SecondRate;
			ceasar.DamageCapacity = 14;
			ceasar.Marines = 4;
			ceasar.RateDamaged = RelativeRate.FourthRate;
			ceasar.DamageCapacityDamaged = 13;
			ceasar.MarinesDamaged = 3;
			ceasar = ceasar.Save();

			Ship hero = Ship.NewShip("Hero");
			hero.Nationality = Nationality.British;
			hero.VictoryPoints = 5;
			hero.Rate = RelativeRate.ThirdRate;
			hero.DamageCapacity = 11;
			hero.Marines = 4;
			hero.RateDamaged = RelativeRate.FifthRate;
			hero.DamageCapacityDamaged = 11;
			hero.MarinesDamaged = 3;
			hero = hero.Save();

			Ship courageux = Ship.NewShip("Courageux");
			courageux.Nationality = Nationality.British;
			courageux.VictoryPoints = 6;
			courageux.Rate = RelativeRate.ThirdRateWhiteCircle;
			courageux.DamageCapacity = 13;
			courageux.Marines = 4;
			courageux.RateDamaged = RelativeRate.FifthRateWhiteCircle;
			courageux.DamageCapacityDamaged = 12;
			courageux.MarinesDamaged = 3;
			courageux = courageux.Save();

			Ship dugay = Ship.NewShip("Dugay-Trouin");
			dugay.Nationality = Nationality.French;
			dugay.VictoryPoints = 6;
			dugay.Rate = RelativeRate.ThirdRateWhiteCircle;
			dugay.DamageCapacity = 13;
			dugay.Marines = 4;
			dugay.RateDamaged = RelativeRate.FifthRateWhiteCircle;
			dugay.DamageCapacityDamaged = 12;
			dugay.MarinesDamaged = 3;
			dugay = dugay.Save();

			Ship formidable = Ship.NewShip("Formidable");
			formidable.Nationality = Nationality.French;
			formidable.VictoryPoints = 7;
			formidable.Rate = RelativeRate.FirstRateBlackCircle;
			formidable.DamageCapacity = 15;
			formidable.Marines = 4;
			formidable.RateDamaged = RelativeRate.FourthRateBlackCircle;
			formidable.DamageCapacityDamaged = 14;
			formidable.MarinesDamaged = 3;
			formidable = formidable.Save();

			Ship montBlanc = Ship.NewShip("Mont-Blanc");
			montBlanc.Nationality = Nationality.French;
			montBlanc.VictoryPoints = 6;
			montBlanc.Rate = RelativeRate.ThirdRateWhiteCircle;
			montBlanc.DamageCapacity = 13;
			montBlanc.Marines = 4;
			montBlanc.RateDamaged = RelativeRate.FifthRateWhiteCircle;
			montBlanc.DamageCapacityDamaged = 12;
			montBlanc.MarinesDamaged = 3;
			montBlanc = montBlanc.Save();

			Ship scipion = Ship.NewShip("Scipion");
			scipion.Nationality = Nationality.French;
			scipion.VictoryPoints = 6;
			scipion.Rate = RelativeRate.ThirdRateWhiteCircle;
			scipion.DamageCapacity = 13;
			scipion.Marines = 4;
			scipion.RateDamaged = RelativeRate.FifthRateWhiteCircle;
			scipion.DamageCapacityDamaged = 12;
			scipion.MarinesDamaged = 3;
			scipion = scipion.Save();

			Scenario capeOrtugal = Scenario.NewScenario();
			capeOrtugal.Name = "Cape Ortugal";
			capeOrtugal.Year = 1805;
			capeOrtugal.WindDirection = 6;
			capeOrtugal.WeatherEffects = Weather.Normal;
			capeOrtugal.Turns = 12;

			Fleet british = Fleet.NewFleet();
			british.Team = Nationality.British;
			british.Nationality = Nationality.British;
			british.Audacity = 3;
			british.Ships.Add(FleetShip.NewFleetShip(ceasar));
			british.Ships.Add(FleetShip.NewFleetShip(hero));
			british.Ships.Add(FleetShip.NewFleetShip(courageux));
			capeOrtugal.Fleets.Add(british);

			Fleet french = Fleet.NewFleet();
			french.Team = Nationality.French;
			french.Nationality = Nationality.French;
			french.Audacity = 1;
			french.Ships.Add(FleetShip.NewFleetShip(dugay));
			var formidableFleet = FleetShip.NewFleetShip(formidable);
			formidableFleet.HullHitsAtStart = 6;
			french.Ships.Add(formidableFleet);
			french.Ships.Add(FleetShip.NewFleetShip(montBlanc));
			french.Ships.Add(FleetShip.NewFleetShip(scipion));
			capeOrtugal.Fleets.Add(french);

			capeOrtugal = capeOrtugal.Save();

			ScenarioList list = ScenarioList.GetScenarios();
			Assert.NotEmpty(list);

			capeOrtugal.Name = "Cape Ortugal 2";
			capeOrtugal = capeOrtugal.Save();

			capeOrtugal.Delete();
			capeOrtugal = capeOrtugal.Save();

			ceasar.Delete();
			ceasar = ceasar.Save();

			ShipList ships = ShipList.GetAllShips();
		}
	}
}
