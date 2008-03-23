using System;
using System.Collections;
using System.Reflection;
using log4net;
using log4net.Config;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace NHibernate.Validator.Tests
{
	public abstract class PersistenceTest
	{
		private const bool OutputDdl = false;
		protected NHibernate.Cfg.Configuration cfg;
		protected ISessionFactory sessions;

		private static readonly ILog log = LogManager.GetLogger(typeof(PersistenceTest));

		protected Dialect.Dialect Dialect
		{
			get { return NHibernate.Dialect.Dialect.GetDialect(cfg.Properties); }
		}

		protected ISession lastOpenedSession;

		/// <summary>
		/// Mapping files used in the TestCase
		/// </summary>
		protected abstract IList Mappings { get; }

		/// <summary>
		/// Assembly to load mapping files from (default is NHibernate.DomainModel).
		/// </summary>
		protected virtual string MappingsAssembly
		{
			get { return "NHibernate.Validator.Tests"; }
		}

		static PersistenceTest()
		{
			// Configure log4net here since configuration through an attribute doesn't always work.
			XmlConfigurator.Configure();
		}

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			try
			{
				Configure();

				CreateSchema();
				BuildSessionFactory();
			}
			catch (Exception e)
			{
				log.Error("Error while setting up the test fixture", e);
				throw;
			}
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			DropSchema();
			Cleanup();
		}

		protected virtual void OnSetUp()
		{
		}

		[SetUp]
		public void SetUp()
		{
			OnSetUp();
		}

		protected virtual void OnTearDown()
		{
		}

		[TearDown]
		public void TearDown()
		{
			OnTearDown();

			bool wasClosed = CheckSessionWasClosed();
			bool wasCleaned = CheckDatabaseWasCleaned();
			bool fail = !wasClosed || !wasCleaned;

			if (fail)
			{
				Assert.Fail("Test didn't clean up after itself");
			}
		}

		private bool CheckSessionWasClosed()
		{
			if (lastOpenedSession != null && lastOpenedSession.IsOpen)
			{
				log.Error("Test case didn't close a session, closing");
				lastOpenedSession.Close();
				return false;
			}

			return true;
		}

		private bool CheckDatabaseWasCleaned()
		{
			if (sessions.GetAllClassMetadata().Count == 0)
			{
				// Return early in the case of no mappings, also avoiding
				// a warning when executing the HQL below.
				return true;
			}

			bool empty;
			using (ISession s = sessions.OpenSession())
			{
				IList objects = s.CreateQuery("from System.Object o").List();
				empty = objects.Count == 0;
			}

			if (!empty)
			{
				log.Error("Test case didn't clean up the database after itself, re-creating the schema");
				DropSchema();
				CreateSchema();
			}

			return empty;
		}


		private void Configure()
		{
			cfg = new NHibernate.Cfg.Configuration();
			if (TestConfigurationHelper.hibernateConfigFile != null)
				cfg.Configure(TestConfigurationHelper.hibernateConfigFile);

			Assembly assembly = Assembly.Load(MappingsAssembly);

			foreach (string file in Mappings)
			{
				cfg.AddResource(MappingsAssembly + "." + file, assembly);
			}

			Configure(cfg);
		}

		private void CreateSchema()
		{
			new SchemaExport(cfg).Create(OutputDdl, true);
		}

		private void DropSchema()
		{
			new SchemaExport(cfg).Drop(OutputDdl, true);
		}

		protected virtual void BuildSessionFactory()
		{
			sessions = cfg.BuildSessionFactory();
		}

		private void Cleanup()
		{
			sessions.Close();
			sessions = null;
			lastOpenedSession = null;
			cfg = null;
		}

		protected ISessionFactoryImplementor Sfi
		{
			get { return (ISessionFactoryImplementor)sessions; }
		}

		protected virtual ISession OpenSession()
		{
			lastOpenedSession = sessions.OpenSession();
			return lastOpenedSession;
		}

		#region Properties overridable by subclasses

		protected virtual void Configure(NHibernate.Cfg.Configuration configuration)
		{
		}


		#endregion

	}
}
