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
		}

		private BattleShip _selectedShip = null;
		public BattleShip SelectedShip
		{
			get { return _selectedShip; }
			set
			{
				if (_selectedShip == null ||
					_selectedShip.Name != value.Name)
				{
					_selectedShip = value;

					if (ScopedRegionManager != null)
					{
						var fireAttackRegion = ScopedRegionManager.Regions["FireAttackRegion"];
						var fireAttackView = (FireAttackView)fireAttackRegion.GetView(_selectedShip.Name);
						if (fireAttackView == null)
						{
							fireAttackView = _container.Resolve<FireAttackView>();
							var fireAttackViewModel = (FireAttackViewModel)fireAttackView.DataContext;
							fireAttackViewModel.Model = FireAttack.NewFireAttack(_selectedShip);
							var scopedRegionManager = fireAttackRegion.Add(fireAttackView, _selectedShip.Name, true);
						}
						else
						{
							fireAttackRegion.Activate(fireAttackView);
						}
					}
					_eventAggregator.GetEvent<BattleShipSelectedEvent>().Publish(value);
				}
			}
		}

		public FireAttackViewModel FireAttackViewModel
		{
			get;
			set;
		}

		public IRegionManager ScopedRegionManager { get; set; }
	}
}
