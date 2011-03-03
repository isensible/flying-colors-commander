using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using FlyingColors;
using System.Reflection;
using Castle.ActiveRecord.Framework.Config;
using Castle.ActiveRecord;

namespace FlyingColorsDesktop
{
	class Bootstrapper : UnityBootstrapper
	{
		protected override DependencyObject CreateShell()
		{
			return new Shell();
		}

		protected override void InitializeShell()
		{
			base.InitializeShell();

			SetupDatabase();

			App.Current.MainWindow = (Window)this.Shell;
			App.Current.MainWindow.Show();
		}

		private void SetupDatabase()
		{
			var config = InPlaceConfigurationSource.Build(
				DatabaseType.SQLite,
				"Data Source=flyingcolors.db;Version=3;");
			ActiveRecordStarter.Initialize(Assembly.LoadFrom("FlyingColors.dll"), config);
			//ActiveRecordStarter.CreateSchema();
			ActiveRecordStarter.UpdateSchema();
		}

		protected override void ConfigureModuleCatalog()
		{
			base.ConfigureModuleCatalog();
			ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
			moduleCatalog.AddModule(typeof(BattlesModule));
		}
	}
}
