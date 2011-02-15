using System;
using Castle.ActiveRecord;
using Csla;
using FlyingColors.DataModel;

namespace FlyingColors
{
	[Serializable]
	public class Commander : BusinessBase<Commander>
	{
		#region ID and Name
		public static readonly PropertyInfo<long> CommanderIdProperty = RegisterProperty<long>(c => c.CommanderId);
		/// <Summary>
		/// Gets or sets the CommanderId value.
		/// </Summary>
		public long CommanderId
		{
			get { return GetProperty(CommanderIdProperty); }
			set { SetProperty(CommanderIdProperty, value); }
		}

		public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
		/// <Summary>
		/// Gets or sets the Name value.
		/// </Summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		} 
		#endregion

		#region Nationality and Victory Points
		public static readonly PropertyInfo<Nationality> NationalityProperty = RegisterProperty<Nationality>(c => c.Nationality);
		/// <Summary>
		/// Gets or sets the Nationality value.
		/// </Summary>
		public Nationality Nationality
		{
			get { return GetProperty(NationalityProperty); }
			set { SetProperty(NationalityProperty, value); }
		}

		public static readonly PropertyInfo<int> VictoryPointsProperty = RegisterProperty<int>(c => c.VictoryPoints);
		/// <Summary>
		/// Gets or sets the VictoryPoints value.
		/// </Summary>
		public int VictoryPoints
		{
			get { return GetProperty(VictoryPointsProperty); }
			set { SetProperty(VictoryPointsProperty, value); }
		} 
		#endregion

		#region Rank, Command Quality and Command Range
		public static readonly PropertyInfo<int> RankProperty = RegisterProperty<int>(c => c.Rank);
		/// <Summary>
		/// Gets or sets the Rank value.
		/// </Summary>
		public int Rank
		{
			get { return GetProperty(RankProperty); }
			set { SetProperty(RankProperty, value); }
		}

		public static readonly PropertyInfo<int> QualityProperty = RegisterProperty<int>(c => c.Quality);
		/// <Summary>
		/// Gets or sets the CommandQuality value.
		/// </Summary>
		public int Quality
		{
			get { return GetProperty(QualityProperty); }
			set { SetProperty(QualityProperty, value); }
		}

		public static readonly PropertyInfo<int> QualityWoundedProperty = RegisterProperty<int>(c => c.QualityWounded);
		/// <Summary>
		/// Gets or sets the CommandQualityWounded value.
		/// </Summary>
		public int QualityWounded
		{
			get { return GetProperty(QualityWoundedProperty); }
			set { SetProperty(QualityWoundedProperty, value); }
		}

		public static readonly PropertyInfo<int> RangeProperty = RegisterProperty<int>(c => c.Range);
		/// <Summary>
		/// Gets or sets the CommandRange value.
		/// </Summary>
		public int Range
		{
			get { return GetProperty(RangeProperty); }
			set { SetProperty(RangeProperty, value); }
		}

		public static readonly PropertyInfo<int> RangeWoundedProperty = RegisterProperty<int>(c => c.RangeWounded);
		/// <Summary>
		/// Gets or sets the CommandRangeWounded value.
		/// </Summary>
		public int RangeWounded
		{
			get { return GetProperty(RangeWoundedProperty); }
			set { SetProperty(RangeWoundedProperty, value); }
		}

		#endregion

		#region Factory Methods
		public static Commander NewCommander(string name)
		{
			return DataPortal.Create<Commander>(name);
		}

		internal static Commander GetCommander(CommanderData commanderData)
		{
			return DataPortal.Fetch<Commander>(commanderData);
		}

		private Commander()
		{
		}
		#endregion

		#region Data Portal
		private CommanderData _commander = null;

		private void DataPortal_Create(string name)
		{
			_commander = new CommanderData();
			LoadProperty<string>(NameProperty, name);
			LoadProperty<Nationality>(NationalityProperty, Nationality.British);
			LoadProperty<int>(VictoryPointsProperty, 0);
			LoadProperty<int>(RankProperty, 0);
			LoadProperty<int>(QualityProperty, 0);
			LoadProperty<int>(QualityWoundedProperty, 0);
			LoadProperty<int>(RangeProperty, 0);
			LoadProperty<int>(RangeWoundedProperty, 0);
		}

		private void DataPortal_Fetch(CommanderData commanderData)
		{
			_commander = commanderData;
			LoadProperty<long>(CommanderIdProperty, _commander.CommanderId);
			LoadProperty<string>(NameProperty, _commander.Name);
			LoadProperty<Nationality>(NationalityProperty,
				(Nationality)Enum.Parse(typeof(Nationality), _commander.Nationality));
			LoadProperty<int>(VictoryPointsProperty, _commander.VictoryPoints);
			LoadProperty<int>(RankProperty, _commander.Rank);
			LoadProperty<int>(QualityProperty, _commander.Quality);
			LoadProperty<int>(QualityWoundedProperty, _commander.QualityWounded);
			LoadProperty<int>(RangeProperty, _commander.Range);			
			LoadProperty<int>(RangeWoundedProperty, _commander.RangeWounded);
		}

		protected override void DataPortal_Insert()
		{
			ToData();
			ActiveRecordMediator<CommanderData>.Create(_commander);
			LoadProperty<long>(CommanderIdProperty, _commander.CommanderId);
		}

		protected override void DataPortal_Update()
		{
			ToData();
			ActiveRecordMediator<CommanderData>.Update(_commander);
		}

		protected override void DataPortal_DeleteSelf()
		{
			ActiveRecordMediator<CommanderData>.Delete(_commander);
		}

		internal CommanderData ToData()
		{
			_commander.Name = ReadProperty<string>(NameProperty);
			_commander.Nationality = ReadProperty<Nationality>(NationalityProperty).ToString();
			_commander.VictoryPoints = ReadProperty<int>(VictoryPointsProperty);
			_commander.Rank = ReadProperty<int>(RankProperty);
			_commander.Quality = ReadProperty<int>(QualityProperty);
			_commander.QualityWounded = ReadProperty<int>(QualityWoundedProperty);
			_commander.Range = ReadProperty<int>(RangeProperty);
			_commander.RangeWounded = ReadProperty<int>(RangeWoundedProperty);
			return _commander;
		}
		#endregion
	}
}
