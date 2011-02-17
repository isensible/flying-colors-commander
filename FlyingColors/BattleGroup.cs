using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;

namespace FlyingColors
{
	[Serializable]
	public class BattleGroup : BusinessBase<BattleGroup>
	{
		public static PropertyInfo<Nationality> NationalityProperty = RegisterProperty<Nationality>(c => c.Nationality);
		public Nationality Nationality
		{
			get { return GetProperty(NationalityProperty); }
			private set { LoadProperty(NationalityProperty, value); }
		}

		public static PropertyInfo<BattleShipList> ShipsProperty = RegisterProperty<BattleShipList>(c => c.Ships);
		public BattleShipList Ships
		{
			get { return GetProperty(ShipsProperty); }
			set { SetProperty(ShipsProperty, value); }
		}
	}
}
