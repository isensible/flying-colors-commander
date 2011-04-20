using FlyingColors;
using FlyingColorsDesktop.Common;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using FlyingColorsDesktop.Events;
using FlyingColorsDesktop.Views;

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

		protected override void OnModelChanged(BattleGroup oldValue, BattleGroup newValue)
		{
			base.OnModelChanged(oldValue, newValue);
			var fireAttackRegion = _regionManager.Regions["FireAttackRegion"];
			var fireAttackView = _container.Resolve<FireAttackView>();
			var scopedRegionManager = fireAttackRegion.Add(fireAttackView, "FireAttack", true);
		}

		private BattleShip _selectedShip = null;
		public BattleShip SelectedShip
		{
			get { return _selectedShip; }
			set
			{
				if (_selectedShip.Name != value.Name)
				{
					_selectedShip = value;

					_eventAggregator.GetEvent<BattleShipSelectedEvent>().Publish(value);
				}
			}
		}

		public FireAttackViewModel FireAttackViewModel
		{
			get;
			set;
		}
	}
}
