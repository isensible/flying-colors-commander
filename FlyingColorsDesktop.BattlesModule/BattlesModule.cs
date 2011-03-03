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

		public BattlesModule(IRegionManager regionManager, IUnityContainer container)
		{
			_regionManager = regionManager;
			_container = container;
		}

		public void Initialize()
		{
			_container.RegisterType<BattleViewModel>();
			_regionManager.RegisterViewWithRegion("MainRegion", typeof(BattleView));		
		}
	}
}
