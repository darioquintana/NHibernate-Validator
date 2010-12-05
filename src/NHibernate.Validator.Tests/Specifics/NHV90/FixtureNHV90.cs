using System.Collections;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Event;
using NHibernate.Validator.Exceptions;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Specifics.NHV90
{
	[TestFixture,Ignore("Not fixed yet")]
	public class FixtureNHV90 : PersistenceTest
	{
		#region Setup/Teardown

		[TearDown]
		public void CleanUp()
		{
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				s.Delete("from TheParent p");
				s.Delete("from TheChild c");
				t.Commit();
			}
		}

		#endregion

		protected override IList Mappings
		{
			get { return new[] {"Specifics.NHV90.Model.hbm.xml"}; }
		}

		protected ValidatorEngine vtor;

		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			Environment.SharedEngineProvider = new NHibernateSharedEngineProvider();
			vtor = Environment.SharedEngineProvider.GetEngine();
			vtor.Clear();
			var nhvc = new XmlConfiguration();
			nhvc.Properties[Environment.ApplyToDDL] = "false";
			nhvc.Properties[Environment.AutoregisterListeners] = "true";
			nhvc.Properties[Environment.ValidatorMode] = "UseAttribute";
			vtor.Configure(nhvc);

			configuration.Initialize();
		}

		[Test]
		public void When_removing_the_last_child_the_parent_should_not_be_valid()
		{
			var p = new TheParent("x");
			p.Children.Add(new TheChild("kik"));
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.SaveOrUpdate(p);
				tx.Commit();
			}

			TheParent loadedParent;
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				loadedParent = s.CreateQuery("from TheParent p where p.Name = 'x'")
					.UniqueResult<TheParent>();

				loadedParent.Children.Clear();

				// This currently passes.
				Assert.False(vtor.IsValid(loadedParent));

				// this currently fails.
				tx.Executing(t => t.Commit()).Throws<InvalidStateException>();
			}
		}
	}
}