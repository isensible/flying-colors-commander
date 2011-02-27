using System.Reflection;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using Xunit;

namespace FlyingColors.Test
{
	public class UseSqliteAttribute : BeforeAfterTestAttribute
	{
		public UseSqliteAttribute()
		{
		}

		public override void Before(MethodInfo methodUnderTest)
		{
			var config = InPlaceConfigurationSource.Build(
				DatabaseType.SQLite,
				"Data Source=flyingcolors.db;Version=3;");
			ActiveRecordStarter.Initialize(Assembly.LoadFrom("FlyingColors.dll"), config);
			//ActiveRecordStarter.CreateSchema();
			ActiveRecordStarter.UpdateSchema();
			
		}

		public override void After(MethodInfo methodUnderTest)
		{
			ActiveRecordStarter.DropSchema();
		}
	}
}
