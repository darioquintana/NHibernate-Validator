using System.Collections;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Event;
using NUnit.Framework;
using System.Collections.Generic;

namespace NHibernate.Validator.Tests.DeepIntegration
{
	public abstract class AbstractMultipleCollectionFixture : PersistenceTest
	{
		protected abstract void AddToCollection(ICollection collection, Person person);
		protected abstract void AddToCollection(ICollection<Person> collection, Person person);
		protected abstract ICollection CreateCollection();
		protected abstract ICollection<Person> GCreateCollection();

		private Person CreateGrandparent()
		{
			Person parent = new Person("GP");
			parent.Children = GCreateCollection();

			for (int i = 0; i < 2; i++)
			{
				Person child = new Person("C" + i);
				child.Parent = parent;
				AddToCollection(parent.Children, child);

				child.Children = GCreateCollection();

				for (int j = 0; j < 3; j++)
				{
					Person grandChild = new Person("C" + i + "-" + j);
					grandChild.Parent = child;
					AddToCollection(child.Children, grandChild);
				}
			}

			parent.Friends = CreateCollection();
			for (int i = 0; i < 3; i++)
			{
				Person friend = new Person("F" + i);
				AddToCollection(parent.Friends, friend);
			}
			return parent;
		}

		protected virtual void RunFetchTest(Person parent)
		{
			// Saved object are valid and don't need to initialize it for validation matters
			using (ISession s = OpenSession())
			using(ITransaction tx = s.BeginTransaction())
			{
				s.Save(parent);
				tx.Commit();
			}
			using (ISession s = OpenSession())
			{
				Person p = s.CreateQuery("from Person p where p.Name = 'GP'")
					.UniqueResult<Person>();

				// This two lines are not needed for tests because is a NH matter
				// use it only to check where the problem really is
				Assert.IsFalse(NHibernateUtil.IsInitialized(p.Children));
				Assert.IsFalse(NHibernateUtil.IsInitialized(p.Friends));
				
				vengine.Validate(p);
				
				Assert.IsFalse(NHibernateUtil.IsInitialized(p.Children));
				Assert.IsFalse(NHibernateUtil.IsInitialized(p.Friends));
			}
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				s.Delete("from Person p where p.Parent is null");
				t.Commit();
			}
		}

		protected ValidatorEngine vengine;
		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			Environment.SharedEngineProvider = new NHibernateSharedEngineProvider();
			vengine = Environment.SharedEngineProvider.GetEngine();
			vengine.Clear();
			NHVConfiguration nhvc = new NHVConfiguration();
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

		[Test]
		public void NoInitializeAfterFetch()
		{
			Person parent = CreateGrandparent();
			RunFetchTest(parent);
		}
	}
}