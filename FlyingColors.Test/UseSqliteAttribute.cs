using System;  
using System.Collections;  
using System.Data;  
using System.Reflection;  
using Castle.ActiveRecord;  
using Castle.ActiveRecord.Framework;  
using Castle.ActiveRecord.Framework.Config;  
using NHibernate.Connection;  
using Xunit;
using System.Collections.Generic;

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
			ActiveRecordStarter.CreateSchema();
			ActiveRecordStarter.UpdateSchema();
			
		}

		public override void After(MethodInfo methodUnderTest)
		{
			ActiveRecordStarter.DropSchema();
		}
	}

	public class UseInMemoryDatabaseAttribute : BeforeAfterTestAttribute
	{
		public UseInMemoryDatabaseAttribute()
		{
		}

		public override void Before(MethodInfo methodUnderTest)
		{
			ActiveRecordMockConnectionProvider.InitializeActiveRecord(true, Assembly.LoadFrom("FlyingColors.dll"));
		}

		public override void After(MethodInfo methodUnderTest)
		{
			ActiveRecordMockConnectionProvider.ExplicitlyDestroyConnection();
		}
	}

	public class ActiveRecordMockConnectionProvider : DriverConnectionProvider
	{
		private static IDbConnection _connection;

		private static IConfigurationSource MockConfiguration
		{
			get
			{
				//var properties = new Dictionary<string, string>  
				//{  
				//    {"hibernate.connection.driver_class", "NHibernate.Driver.SQLite20Driver"},  
				//    {"hibernate.dialect", "NHibernate.Dialect.SQLiteDialect"},  
				//    {"hibernate.connection.provider", ConnectionProviderLocator},  
				//    {"hibernate.connection.connection_string", "Data Source=:memory:;Version=3;New=True;"}  
				//};

				//var source = new InPlaceConfigurationSource();
				//source.Add(typeof(ActiveRecordBase), properties);

				var config = InPlaceConfigurationSource.Build(
				DatabaseType.SQLite,
				"Data Source=flyingcolors.db;Version=3;");

				return config;
			}
		}

		private static string ConnectionProviderLocator
		{
			get
			{
				return String.Format("{0}, {1}", TypeOfEnclosingClass.FullName,
										EnclosingAssemblyName.Split(',')[0]);
			}
		}

		private static Type TypeOfEnclosingClass
		{
			get { return MethodBase.GetCurrentMethod().DeclaringType; }
		}

		private static string EnclosingAssemblyName
		{
			get { return Assembly.GetAssembly(TypeOfEnclosingClass).FullName; }
		}

		public override IDbConnection GetConnection()
		{
			if (_connection == null)
				_connection = base.GetConnection();

			return _connection;
		}

		public override void CloseConnection(IDbConnection conn) { }

		/// <summary>  
		/// Destroys the connection that is kept open in order to keep the  
		/// in-memory database alive. Destroying the connection will destroy  
		/// all of the data stored in the mock database. Call this method when  
		/// the test is complete.  
		/// </summary>  
		public static void ExplicitlyDestroyConnection()
		{
			if (_connection != null)
			{
				_connection.Close();
				_connection = null;
			}
		}

		/// <summary>  
		/// Initializes ActiveRecord and the Database that ActiveRecord uses to  
		/// store the data. Call this method before the test executes.  
		/// </summary>  
		/// <param name="useDynamicConfiguration">  
		/// Use reflection to build configuration, rather than the Configuration  
		/// file.  
		/// </param>  
		/// <param name="types">  
		/// A list of ActiveRecord types that will be created in the database  
		/// </param>  
		public static void InitializeActiveRecord(bool useDynamicConfiguration,
												  params Type[] types)
		{
			ActiveRecordStarter.ResetInitializationFlag();
			IConfigurationSource configurationSource = useDynamicConfiguration
											 ? MockConfiguration
											 : ActiveRecordSectionHandler.Instance;
			ActiveRecordStarter.Initialize(configurationSource, types);
			ActiveRecordStarter.CreateSchema();
		}

		public static void InitializeActiveRecord(bool useDynamicConfiguration, Assembly asm)
		{
			ActiveRecordStarter.ResetInitializationFlag();
			IConfigurationSource configurationSource = useDynamicConfiguration
											 ? MockConfiguration
											 : ActiveRecordSectionHandler.Instance;
			ActiveRecordStarter.Initialize(asm, configurationSource);
			ActiveRecordStarter.CreateSchema();			
		}
													

		/// <summary>  
		/// Initializes ActiveRecord and the Database that ActiveRecord uses to  
		/// store the data based. Configuration is dynamically generated using  
		/// reflection. Call this method before the test executes.  
		/// </summary>  
		/// <param name="types">  
		/// A list of ActiveRecord types that will be created in the database  
		/// </param>  
		[Obsolete("Use InitializeActiveRecord(bool, params Type[])")]
		public static void InitializeActiveRecord(params Type[] types)
		{
			InitializeActiveRecord(true, types);
		}
	}  
}
