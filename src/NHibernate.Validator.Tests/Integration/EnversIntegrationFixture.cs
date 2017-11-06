using System.Collections;

using NHibernate.Cfg;
using NHibernate.Envers.Configuration.Fluent;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Event;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Tests.Base;

using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Integration
{
	class EnversIntegrationFixture : PersistenceTest
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]
				{
					"Integration.Address.hbm.xml",
				};
			}
		}

		protected ISharedEngineProvider fortest;
		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			// The ValidatorInitializer and the ValidateEventListener share the same engine

			// Initialize the SharedEngine
			fortest = new NHibernateSharedEngineProvider();
			Cfg.Environment.SharedEngineProvider = fortest;
			ValidatorEngine ve = Cfg.Environment.SharedEngineProvider.GetEngine();
			ve.Clear();

			XmlConfiguration nhvc = new XmlConfiguration();
			nhvc.Properties[Cfg.Environment.ApplyToDDL] = "true";
			nhvc.Properties[Cfg.Environment.AutoregisterListeners] = "true";
			nhvc.Properties[Cfg.Environment.ValidatorMode] = "UseAttribute";
			nhvc.Properties[Cfg.Environment.MessageInterpolatorClass] = typeof(PrefixMessageInterpolator).AssemblyQualifiedName;

			var enversConf = new FluentConfiguration();
			enversConf.Audit<Address>();
			configuration.IntegrateWithEnvers(enversConf);

			ve.Configure(nhvc);

			ValidatorInitializer.Initialize(configuration);
		}


		[Test]
		public void ShouldWorkSUIDWithEnversAndValidators()
		{
			ISession s;
			Address.blacklistedZipCode = "3232";

			// Don't throw exception if it is valid
			Address a = new Address();

			a.Id = 13;
			a.Country = "Country";
			a.Line1 = "Line 1";
			a.Zip = "4343";
			a.State = "NY";
			try
			{
				using (s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					s.Save(a);
					t.Commit();
				}
			}
			catch (InvalidStateException)
			{
				Assert.Fail("Valid entity cause InvalidStateException");
			}

			// Update check
			try
			{
				using (s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Address saved = s.Get<Address>(13L);
					saved.State = "TOOLONG";
					s.Update(saved);
					t.Commit();
					Assert.Fail("entity should have been validated");
				}
			}
			catch (InvalidStateException e)
			{
				e.GetInvalidValues().Should().Not.Be.Empty();
			}

			try
			{
				using (s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Address saved = s.Get<Address>(13L);
					a.Zip = "1234";
					saved.State = "BO";
					s.Update(saved);
					t.Commit();
				}
			}
			catch (InvalidStateException)
			{
				Assert.Fail("Valid entity cause InvalidStateException");
			}


			// clean up
			using (s = OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					Address saved = s.Get<Address>(13L);
					s.Delete(saved);
					t.Commit();
				}

				var c = s.Connection.CreateCommand();
				c.CommandText = "DELETE FROM Address_AUD; DELETE FROM REVINFO;";
				c.ExecuteScalar();

			}
		}
	}
}
