using System.Collections;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Event;
using NHibernate.Validator.Exceptions;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.DeepIntegration
{
	[TestFixture]
	public class DynamicComponentFixture : PersistenceTest
	{
		protected override IList Mappings
		{
			get
			{
				return new string[] { "DeepIntegration.PersonBag.hbm.xml", "DeepIntegration.DynaEntity.hbm.xml" };
			}
		}

		protected ValidatorEngine vengine;
		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			Environment.SharedEngineProvider = new NHibernateSharedEngineProvider();
			vengine = Environment.SharedEngineProvider.GetEngine();
			vengine.Clear();
			XmlConfiguration nhvc = new XmlConfiguration();
			nhvc.Properties[Environment.ApplyToDDL] = "false";
			nhvc.Properties[Environment.AutoregisterListeners] = "true";
			nhvc.Properties[Environment.ValidatorMode] = "UseAttribute";
			vengine.Configure(nhvc);

			ValidatorInitializer.Initialize(configuration);
		}

		protected override void OnTestFixtureTearDown()
		{
			// reset the engine
			Environment.SharedEngineProvider = null;
		}

		private DynaEntity InitializeNewDyna()
		{
			DynaEntity de = new DynaEntity();
			de.DynaBean = new Hashtable();
			de.DynaBean["TheName"] = "Arturius";
			de.DynaBean["Person"] = null;
			object savedId;
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				savedId = s.Save(de);
				tx.Commit();
			}
			DynaEntity result;
			using (ISession s = OpenSession())
			{
				result = s.Get<DynaEntity>(savedId);
			}
			return result;
		}

		private void CleanUp()
		{
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				s.Delete("from DynaEntity");
				s.Delete("from Person p where p.Parent is null");
				t.Commit();
			}
		}

		[Test]
		public void CointainInvalidValues()
		{
			DynaEntity de = InitializeNewDyna();
			de.DynaBean["Person"] = new Person("A");
			try
			{
				using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					s.Update(de);
					tx.Commit();
				}
				Assert.Fail("The DynaEntity should be NO valid");
			}
			catch (InvalidStateException)
			{
				// ok
			}

			CleanUp();
		}
	}
}
