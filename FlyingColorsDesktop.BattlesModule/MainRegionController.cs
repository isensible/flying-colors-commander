using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Events;
using FlyingColorsDesktop.Events;
using FlyingColors;
using FlyingColorsDesktop.Views;
using FlyingColorsDesktop.ViewModels;

namespace FlyingColorsDesktop
{
	public class MainRegionController
	{
		private readonly IUnityContainer _container = null;
		private readonly IRegionManager _regionManager = null;
		private readonly IEventAggregator _eventAggregator = null;

		public MainRegionController(
			IUnityContainer container,
			IRegionManager regionManager,
			IEventAggregator eventAggregator)
		{
			_container = container;
			_regionManager = regionManager;
			_eventAggregator = eventAggregator;
			_eventAggregator.GetEvent<BattleSelectedEvent>().Subscribe(OnBattleSelected, true);
		}

		private void OnBattleSelected(Battle battle)
		{
			if (battle == null) return;

			IRegion mainRegion = _regionManager.Regions["MainRegion"];
			if (mainRegion == null) return;

			BattleView battleView = (BattleView)mainRegion.GetView(battle.Name);
			if (battleView == null)
			{
				battleView = _container.Resolve<BattleView>();
				var scopedRegionManager = mainRegion.Add(battleView, battle.Name, true);

				var battleGroupAView = _container.Resolve<BattleGroupView>();
				BattleGroupViewModel groupAModel = (BattleGroupViewModel)battleGroupAView.DataContext;
				groupAModel.Model = battle.BattleGroupA;

				var groupARegion = scopedRegionManager.Regions["BattleGroupARegion"];
				var scopedGroupARegionManager =
					groupARegion.Add(battleGroupAView, battle.BattleGroupA.BattleGroupId.ToString(), true);
				battleGroupAView.ScopedRegionManager = scopedGroupARegionManager;
				groupAModel.ScopedRegionManager = scopedGroupARegionManager;
				//scopedRegionManager.AddToRegion("BattleGroupARegion", battleGroupAView);

				var battleGroupBView = _container.Resolve<BattleGroupView>();
				BattleGroupViewModel groupBModel = (BattleGroupViewModel)battleGroupBView.DataContext;
				groupBModel.Model = battle.BattleGroupB;

				var groupBRegion = scopedRegionManager.Regions["BattleGroupBRegion"];
				var scopedGroupBRegionManager =
					groupBRegion.Add(battleGroupBView, battle.BattleGroupB.BattleGroupId.ToString(), true);
				battleGroupBView.ScopedRegionManager = scopedGroupBRegionManager;
				groupBModel.ScopedRegionManager = scopedGroupBRegionManager;
				//scopedRegionManager.AddToRegion("BattleGroupBRegion", battleGroupBView); 
			}
			else
			{
				mainRegion.Activate(battleView);
			}

			BattleViewModel battleViewModel = (BattleViewModel)battleView.DataContext;
			if (battleViewModel != null)
			{
				battleViewModel.Model = battle;
			}
		}

	}
}
