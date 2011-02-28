using FlyingColors;
using FlyingColorsDesktop.Common;

namespace FlyingColorsDesktop.ViewModels
{
	class BattleShipViewModel : ViewModelBase<BattleShip>
	{
		public BattleShipViewModel(BattleShip battleShip)
		{
			Model = battleShip;
		}
	}
}
