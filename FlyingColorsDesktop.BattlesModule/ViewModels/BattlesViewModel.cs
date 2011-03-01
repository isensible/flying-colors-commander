using FlyingColors;
using FlyingColorsDesktop.Common;

namespace FlyingColorsDesktop.ViewModels
{
	public class BattlesViewModel : ViewModelBase<BattleList>
	{
		public BattlesViewModel()
		{
			Model = BattleList.GetBattles();
		}
	}
}
