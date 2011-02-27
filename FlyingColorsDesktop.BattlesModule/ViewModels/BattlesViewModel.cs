using FlyingColors;
using FlyingColorsDesktop.Common;

namespace FlyingColorsDesktop.ViewModels
{
	class BattlesViewModel : ViewModelBase<BattleList>
	{
		public BattlesViewModel()
		{
			Model = BattleList.GetBattles();
		}
	}
}
