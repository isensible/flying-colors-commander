using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;

namespace FlyingColorsDesktop.Controllers
{
	public class BattleGroupsController
	{
		private readonly IUnityContainer _container;
		private readonly IRegionManager _regionManager;
		private readonly IEventAggregator _eventAggregator;

		public BattleGroupsController(IRegionManager regionManager, IEventAggregator eventAggregator, IUnityContainer container)
		{
			_container = container;
			_regionManager = regionManager;
			_eventAggregator = eventAggregator;
		}

		// TODO: coordinate the battle group views.
	}
}
