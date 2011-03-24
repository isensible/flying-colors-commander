using FlyingColorsDesktop.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using FlyingColorsDesktop.ViewModels;

namespace FlyingColorsDesktop
{
	public class BattlesModule : IModule
	{
		private readonly IRegionManager _regionManager;
		private readonly IUnityContainer _container;

		private MainRegionController _mainRegionController = null;

		public BattlesModule(IRegionManager regionManager, IUnityContainer container)
		{
			_regionManager = regionManager;
			_container = container;
		}

		public void Initialize()
		{
			_container.RegisterType<BattlesToolBarViewModel>();
			_regionManager.RegisterViewWithRegion("GlobalCommandsRegion", typeof(BattlesToolBarView));

			_mainRegionController = _container.Resolve<MainRegionController>();

			_container.RegisterType<BattleView>();
			_container.RegisterType<BattleViewModel>();
			//_regionManager.RegisterViewWithRegion("MainRegion", typeof(BattleView));
			_container.RegisterType<BattleGroupView>();
			_container.RegisterType<BattleGroupViewModel>();
			//_regionManager.RegisterViewWithRegion("BattleGroupARegion", typeof(BattleGroupView));
			//_regionManager.RegisterViewWithRegion("BattleGroupBRegion", typeof(BattleGroupView));

			
		}
	}
}
