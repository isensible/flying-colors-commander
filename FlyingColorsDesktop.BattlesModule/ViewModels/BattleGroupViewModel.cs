using FlyingColors;
using FlyingColorsDesktop.Common;

namespace FlyingColorsDesktop.ViewModels
{
	class BattleGroupViewModel : ViewModelBase<BattleGroup>
	{
		public BattleGroupViewModel(BattleGroup battleGroup)
		{
			Model = battleGroup;
		}
	}
}
