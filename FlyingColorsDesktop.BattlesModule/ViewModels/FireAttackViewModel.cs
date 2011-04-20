using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlyingColorsDesktop.Common;
using FlyingColors;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Events;

namespace FlyingColorsDesktop.ViewModels
{
	public class FireAttackViewModel : ViewModelBase<FireAttack>
	{
		private readonly IUnityContainer _container = null;
		private readonly IRegionManager _regionManager = null;
		private readonly IEventAggregator _eventAggregator = null;

		public FireAttackViewModel(
			IUnityContainer container,
			IRegionManager regionManager,
			IEventAggregator eventAggregator)
		{
			_container = container;
			_regionManager = regionManager;
			_eventAggregator = eventAggregator;
			ManageObjectLifetime = false;

		}


	}
}
