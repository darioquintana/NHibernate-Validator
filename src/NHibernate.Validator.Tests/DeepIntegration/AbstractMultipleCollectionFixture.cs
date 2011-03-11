using System.Collections;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Event;
using NHibernate.Validator.Util;
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

		[Test]
		public void NoInitializeAfterFetch()
		{
			// Saved object are valid and don't need to initialize it for validation matters

			Person parent = CreateGrandparent();
			SavePerson(parent);
			int savedId = parent.Id;
			using (ISession s = OpenSession())
			{
				Person p = s.CreateQuery("from Person p where p.Name = 'GP'")
					.UniqueResult<Person>();

				// This two lines are not needed for tests because is a NH matter
				// we use it only to check where the problem really is
				Assert.IsFalse(NHibernateHelper.IsInitialized(p.Children));
				Assert.IsFalse(NHibernateHelper.IsInitialized(p.Friends));

				vengine.Validate(p);

				Assert.IsFalse(NHibernateHelper.IsInitialized(p.Children));
				Assert.IsFalse(NHibernateHelper.IsInitialized(p.Friends));
			}

			// No initialized many-to-one
			using (ISession s = OpenSession())
			{
				Person p = s.CreateQuery("from Person p where p.Name = 'C1'")
					.UniqueResult<Person>();

				// This line are not needed for tests because is a NH matter
				// we use it only to check where the problem really is
				Assert.IsFalse(NHibernateHelper.IsInitialized(p.Parent));

				vengine.Validate(p);

				Assert.IsFalse(NHibernateHelper.IsInitialized(p.Parent));
			}

			// No initialized the proxie it self
			using (ISession s = OpenSession())
			{
				Person p = s.Load<Person>(savedId);

				Assert.IsFalse(NHibernateHelper.IsInitialized(p));

				vengine.Validate(p);

				Assert.IsFalse(NHibernateHelper.IsInitialized(p));
			}

			CleanUp();
		}

		private void CleanUp()
		{
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				s.Delete("from Person p where p.Parent is null");
				t.Commit();
			}
		}

		private void SavePerson(Person parent)
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.Save(parent);
				tx.Commit();
			}
		}

		[Test]
		public void InvalidValuesInCollections()
		{
			// we are testing it using proxies created by NH
			Person parent = CreateGrandparent();
			SavePerson(parent);

			// Generic collection
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				Person p = s.CreateQuery("from Person p where p.Name = 'GP'")
					.UniqueResult<Person>();
				IEnumerator<Person> ep = p.Children.GetEnumerator();
				ep.MoveNext();
				Person aChildren = ep.Current;
				IEnumerator<Person> ep1 = aChildren.Children.GetEnumerator();
				ep1.MoveNext();
				Person aCh11 = ep1.Current;

				Assert.IsTrue(vengine.IsValid(p));

				aCh11.Name = null;

				Assert.IsFalse(vengine.IsValid(p));

				tx.Rollback();
			}

			// No generic collection
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				Person p = s.CreateQuery("from Person p where p.Name = 'GP'")
					.UniqueResult<Person>();
				IEnumerator ep = p.Friends.GetEnumerator();
				ep.MoveNext();
				Person aFriend = (Person)ep.Current;
				Assert.IsTrue(vengine.IsValid(p));

				aFriend.Name = "A";

				Assert.IsFalse(vengine.IsValid(p));

				tx.Rollback();
			}

			// Many-to-one
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				Person p = s.CreateQuery("from Person p where p.Name = 'C1'")
					.UniqueResult<Person>();
				NHibernateUtil.Initialize(p.Parent);

				Assert.IsTrue(vengine.IsValid(p));

				p.Parent.Name = "A";

				Assert.IsFalse(vengine.IsValid(p));

				tx.Rollback();
			}
			CleanUp();
		}

		[Test]
		public void InvalidCollection()
		{
			// we are testing that work for Bag, List, Set PersistentCollections
			Person parent = CreateGrandparent();
			SavePerson(parent);

			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				Person p = s.CreateQuery("from Person p where p.Name = 'GP'")
					.UniqueResult<Person>();

				NHibernateUtil.Initialize(p.Children);

				Assert.IsTrue(vengine.IsValid(p));

				for (int i = 0; i < 10; i++)
				{
					Person child = new Person("CC" + i);
					child.Parent = parent;
					AddToCollection(p.Children, child);
				}

				Assert.IsFalse(vengine.IsValid(p));

				tx.Rollback();
			}

			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				Person p = s.CreateQuery("from Person p where p.Name = 'GP'")
					.UniqueResult<Person>();

				NHibernateUtil.Initialize(p.Friends);

				Assert.IsTrue(vengine.IsValid(p));

				for (int i = 0; i < 3; i++)
				{
					Person friend = new Person("FF" + i);
					AddToCollection(p.Friends, friend);
				}

				Assert.IsFalse(vengine.IsValid(p));

				tx.Rollback();
			}

			CleanUp();
		}
	}
}