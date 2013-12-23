using System.Linq;
using FlyingColors;
using FlyingColorsDesktop.Common;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using FlyingColorsDesktop.Events;

namespace FlyingColorsDesktop.ViewModels
{
	public class BattlesToolBarViewModel : ViewModelBase<ScenarioList>
	{
		private readonly IRegionManager _regionManager = null;
		private readonly IUnityContainer _container = null;
		private readonly IEventAggregator _eventAggregator = null;

		public BattlesToolBarViewModel(
			IUnityContainer container,
			IRegionManager regionManager,
			IEventAggregator eventAggregator)
		{
			_container = container;
			_regionManager = regionManager;
			_eventAggregator = eventAggregator;
			NewBattleCommand = new DelegateCommand<object>(OnNewBattle, CanNewBattle);
			OpenBattleCommand = new DelegateCommand<object>(OnOpenBattle, CanOpenBattle);
			SaveBattleCommand = new DelegateCommand<object>(OnSaveBattle, CanSaveBattle);
			Model = ScenarioList.GetScenarios();
		}

		#region New Battle Command
		public ICommand NewBattleCommand { get; private set; }

		public bool CanNewBattle(object obj)
		{
			return true;
		}

		public void OnNewBattle(object obj)
		{
            
            if (!Model.Any())
                SetupBattle();

		    Model = ScenarioList.GetScenarios();
		    Scenario s1 = Model.First();
            Battle b1 = Battle.NewBattle(s1);
			_eventAggregator.GetEvent<BattleSelectedEvent>().Publish(b1);
		} 
		#endregion

		#region Open Battle Command
		public ICommand OpenBattleCommand { get; private set; }

		public bool CanOpenBattle(object obj)
		{
			return true;
		}

		public void OnOpenBattle(object obj)
		{
			// launch open battle dialog.
		} 
		#endregion

		#region Save Battle Command
		public ICommand SaveBattleCommand { get; private set; }

		public bool CanSaveBattle(object obj)
		{
			return true;
		}

		public void OnSaveBattle(object obj)
		{
			// save the current battle.
		}

		#endregion


        #region Setup Battle
        private Battle SetupBattle()
        {
            Commander strachan = Commander.NewCommander("Strachan");
            strachan.Nationality = Nationality.British;
            strachan.VictoryPoints = 5;
            strachan.Rank = 3;
            strachan.Quality = 2;
            strachan.Range = 6;
            strachan.QualityWounded = 1;
            strachan.RangeWounded = 4;
            strachan = strachan.Save();

            Commander dumanoir = Commander.NewCommander("Dumanoir");
            dumanoir.Nationality = Nationality.French;
            dumanoir.VictoryPoints = 5;
            dumanoir.Rank = 3;
            dumanoir.Quality = 2;
            dumanoir.Range = 6;
            dumanoir.QualityWounded = 1;
            dumanoir.RangeWounded = 4;
            dumanoir = dumanoir.Save();

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
            british.BattleGroup = Nationality.British;
            british.Nationality = Nationality.British;
            british.Audacity = 3;
            var ceasarFleetShip = FleetShip.NewFleetShip(ceasar);
            ceasarFleetShip.Commanders.Add(FleetShipCommander.NewFleetShipCommander(strachan));
            british.Ships.Add(ceasarFleetShip);
            british.Ships.Add(FleetShip.NewFleetShip(hero));
            british.Ships.Add(FleetShip.NewFleetShip(courageux));
            capeOrtugal.Fleets.Add(british);

            Fleet french = Fleet.NewFleet();
            french.BattleGroup = Nationality.French;
            french.Nationality = Nationality.French;
            french.Audacity = 1;
            french.Ships.Add(FleetShip.NewFleetShip(dugay));
            var formidableFleetShip = FleetShip.NewFleetShip(formidable);
            formidableFleetShip.Commanders.Add(FleetShipCommander.NewFleetShipCommander(dumanoir));
            formidableFleetShip.HullHitsAtStart = 6;
            french.Ships.Add(formidableFleetShip);
            french.Ships.Add(FleetShip.NewFleetShip(montBlanc));
            french.Ships.Add(FleetShip.NewFleetShip(scipion));
            capeOrtugal.Fleets.Add(french);

            capeOrtugal = capeOrtugal.Save();

            var battleOfCapeOrtugal = Battle.NewBattle(capeOrtugal);

            battleOfCapeOrtugal = battleOfCapeOrtugal.Save();
            return battleOfCapeOrtugal;
        }
        #endregion
	}
}
