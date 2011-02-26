using FlyingColorsDesktop.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace FlyingColorsDesktop
{
	public class BattlesModule : IModule
	{
		private readonly IRegionManager _regionManager;

		public BattlesModule(IRegionManager regionManager)
		{
			_regionManager = regionManager;
		}

		public void Initialize()
		{
			_regionManager.RegisterViewWithRegion("MainRegion", typeof(BattlesView));
		}
	}
}
