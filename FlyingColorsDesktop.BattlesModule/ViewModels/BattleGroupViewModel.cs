using FlyingColors;
using FlyingColorsDesktop.Common;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;

namespace FlyingColorsDesktop.ViewModels
{
	public class BattleGroupViewModel : ViewModelBase<BattleGroup>
	{
		private readonly IRegionManager _regionManager = null;
		private readonly IUnityContainer _container = null;
		private readonly IEventAggregator _eventAggregator = null;

		public BattleGroupViewModel(
			IUnityContainer container,
			IRegionManager regionManager,
			IEventAggregator eventAggregator)
		{
			_container = container;
			_regionManager = regionManager;
			_eventAggregator = eventAggregator;
			ManageObjectLifetime = false;
			
		}

		public BattleShip SelectedShip { get; set; }
	}
}
