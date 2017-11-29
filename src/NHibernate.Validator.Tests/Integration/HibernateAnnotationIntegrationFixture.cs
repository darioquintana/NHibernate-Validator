using System;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Event;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Tests.Base;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Integration
{
	using System.Collections;
	using Cfg;
	using Mapping;
	using NHibernate.Cfg;
	using NUnit.Framework;

	[TestFixture]
	public class HibernateAnnotationIntegrationFixture : PersistenceTest
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]
					{
						"Integration.Address.hbm.xml",
						"Integration.Tv.hbm.xml",
						"Integration.TvOwner.hbm.xml",
						"Integration.Martian.hbm.xml",
						"Integration.Music.hbm.xml"
					};
			}
		}

		protected class AnyClass
		{
			public int aprop;
		}

		protected ISharedEngineProvider fortest;
		protected override void Configure(Configuration configuration)
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
			ve.Configure(nhvc);
			ve.IsValid(new AnyClass());// add the element to engine for test

			ValidatorInitializer.Initialize(configuration);
		}

		protected override void OnTestFixtureTearDown()
		{
			// reset the engine
			Cfg.Environment.SharedEngineProvider = null;
		}

		public void CleanupData()
		{
			ISession s = OpenSession();
			ITransaction txn = s.BeginTransaction();

			s.Delete("from Address");

			txn.Commit();
			s.Close();
		}

		[Test]
		public virtual void EnsureSharedEngine()
		{
			Assert.IsTrue(ReferenceEquals(fortest, Cfg.Environment.SharedEngineProvider),
						  "some process change the shared engine instance");
			// Have something initialized before and after lister initialization
			Assert.IsNotNull(fortest.GetEngine().GetValidator<AnyClass>());
			Assert.IsNotNull(fortest.GetEngine().GetValidator<Address>());
		}

		[Test]
		public void InvalidInitializer()
		{
			Assert.That(() => ValidatorInitializer.Initialize(null), Throws.TypeOf<ArgumentNullException>());
		}

		[Test]
		public void Apply()
		{
			PersistentClass classMapping = cfg.GetClassMapping(typeof(Address));
			IEnumerator ie1 = classMapping.GetProperty("State").ColumnIterator.GetEnumerator();
			ie1.MoveNext();
			Column stateColumn = (Column) ie1.Current;
			Assert.AreEqual(3, stateColumn.Length);

			IEnumerator ie2 = classMapping.GetProperty("Zip").ColumnIterator.GetEnumerator();
			ie2.MoveNext();
			Column zipColumn = (Column) ie2.Current;
			Assert.AreEqual(5, zipColumn.Length);
			Assert.IsFalse(zipColumn.IsNullable);
		}

		[Test]
		public void ApplyOnIdColumn()
		{
			PersistentClass classMapping = cfg.GetClassMapping(typeof(Tv));
			IEnumerator ie = classMapping.IdentifierProperty.ColumnIterator.GetEnumerator();
			ie.MoveNext();
			Column serialColumn = (Column) ie.Current;
			Assert.AreEqual(2, serialColumn.Length, "Validator annotation not applied on ids");
		}

		[Test]
		public void ApplyOnManyToOne()
		{
			PersistentClass classMapping = cfg.GetClassMapping(typeof(TvOwner));
			IEnumerator ie = classMapping.GetProperty("tv").ColumnIterator.GetEnumerator();
			ie.MoveNext();
			Column serialColumn = (Column) ie.Current;
			Assert.IsFalse(serialColumn.IsNullable, "Validator annotations not applied on associations");
		}

		[Test]
		public void SingleTableAvoidNotNull()
		{
			PersistentClass classMapping = cfg.GetClassMapping(typeof(Rock));
			IEnumerator ie = classMapping.GetProperty("bit").ColumnIterator.GetEnumerator();
			ie.MoveNext();
			Column serialColumn = (Column) ie.Current;
			Assert.IsTrue(serialColumn.IsNullable, "Notnull should not be applied on single tables");
		}

		[Test]
		public void ApplyOnEnumColumn()
		{
			PersistentClass classMapping = cfg.GetClassMapping(typeof(Address));
			IEnumerator ie = classMapping.GetProperty("AddressType").ColumnIterator.GetEnumerator();
			ie.MoveNext();
			Column serialColumn = (Column)ie.Current;
			Assert.AreEqual("AddressType in (0, 1)", serialColumn.CheckConstraint , "Validator annotation shout generate valid check for Enums");

			ie = classMapping.GetProperty("AddressFlags").ColumnIterator.GetEnumerator();
			ie.MoveNext();
			serialColumn = (Column)ie.Current;

			Assert.That(serialColumn.CheckConstraint, Is.Null.Or.Empty, "Validator annotation should not generate check for [Flag]ed Enums");
		}

		/// <summary>
		/// Test pre-update/save events and custom interpolator
		/// </summary>
		[Test]
		public void Events()
		{
			ISession s;
			ITransaction tx;
			Address a = new Address();
			Address.blacklistedZipCode = "3232";
			a.Id = 12;
			a.Country = "Country";
			a.Line1 = "Line 1";
			a.Zip = "nonnumeric";
			a.State = "NY";
			a.AddressType = (AddressType) 42;
			a.AddressFlags = AddressFlag.Flag1 | AddressFlag.Flag2 | (AddressFlag)66;
			s = OpenSession();
			tx = s.BeginTransaction();
			s.Save(a);
			try
			{
				tx.Commit();
				Assert.Fail("entity should have been validated");
			}
			catch(InvalidStateException e)
			{
				//success
				var invalidValues = e.GetInvalidValues();
				Assert.That(invalidValues, Has.Length.EqualTo(4));
				Assert.That(invalidValues, Has.All.Message.StartsWith("prefix_"), "Environment.MESSAGE_INTERPOLATOR_CLASS does not work");
			}
			finally
			{
				if (tx != null && !tx.WasCommitted)
				{
					tx.Rollback();
				}
				s.Close();
			}
			s = OpenSession();
			tx = s.BeginTransaction();
			a.Country = "Country";
			a.Line1 = "Line 1";
			a.Zip = "4343";
			a.State = "NY";
			s.Save(a);
			a.State = "TOOLONG";
			a.AddressType = AddressType.Mailing;
			a.AddressFlags = AddressFlag.Flag1 | AddressFlag.Flag2 | AddressFlag.Flag4; 
			try 
			{
				s.Flush();
				Assert.Fail("entity should have been validated");
			} 
			catch (InvalidStateException e) 
			{
				e.GetInvalidValues().Should().Not.Be.Empty();
			} 
			finally 
			{
				if (tx != null && !tx.WasCommitted) 
					tx.Rollback();
				
				s.Close();
			}

			// Don't throw exception if it is valid
			a = new Address();
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
			catch(InvalidStateException)
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
			using (ITransaction t = s.BeginTransaction())
			{
				Address saved = s.Get<Address>(13L);
				s.Delete(saved);
				t.Commit();
			}
		}

		/// <summary>
		/// Test components and composite-id on validation
		/// </summary>
		[Test]
		public void Components()
		{
			ISession s;
			ITransaction tx;
			s = OpenSession();
			tx = s.BeginTransaction();
			Martian martian = new Martian();
			martian.Id = new MartianPk("Liberal", "Biboudie");
			martian.Address = new MarsAddress("Plus","cont");
			s.Save(martian);
			try 
			{
				s.Flush();
				Assert.Fail("Components are not validated");
			}
			catch (InvalidStateException e) 
			{
				e.GetInvalidValues().Should().Have.Count.EqualTo(2);
			}
			finally 
			{
				tx.Rollback();
				s.Close();
			}
		}

		private class EventCrack : ValidateEventListener
		{
			public static void ValidateCrack(object entity, EntityMode mode)
			{
				Validate(entity, mode);
			}
		}

		[Test]
		public void DontValidate()
		{
			EventCrack.ValidateCrack(null, EntityMode.Poco); // don't throw exception

			Address a = new Address();
			Address.blacklistedZipCode = "3232";
			a.Id = 12;
			a.Country = "Country";
			a.Line1 = "Line 1";
			a.Zip = "nonnumeric";
			a.State = "NY";
			try
			{
				EventCrack.ValidateCrack(a, EntityMode.Poco);
				Assert.Fail("This test don't make sense if we are not sharing the validator engine.");
			}
			catch (InvalidStateException)
			{
				// Ok
			}
			EventCrack.ValidateCrack(a, EntityMode.Map); // don't throw exception
		}
	}
}