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
			Scenario s1 = Model[0];
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
		
	}
}
