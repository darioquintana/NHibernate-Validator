using System.Collections;
using NHibernate.Event;
using NHibernate.Validator.Event;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Integration
{
	[TestFixture]
	public class ValidateEventsFixture : PersistenceTest
	{
		[Test]
		public void NoInitialization()
		{
			// test no problem in case of bad usage out of NH
			ValidatePreInsertEventListener evl = new ValidatePreInsertEventListener();
			evl.Initialize(null);
		}

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

		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			Cfg.Environment.SharedEngineProvider = null;
			cfg.SetListener(ListenerType.PreUpdate, new ValidatePreUpdateEventListener());
		}

		protected override void OnTestFixtureTearDown()
		{
			// reset the engine
			Cfg.Environment.SharedEngineProvider = null;
		}

		/// <summary>
		/// Test pre-update events and custom interpolator without initialization of NH
		/// This test make the same validation without initialization but cat't sure that
		/// the result of validation is the same (the initialization autoregistr subElements for components)
		/// </summary>
		[Test]
		public void UpdateEvent()
		{
			ISession s;
			Address a;
			// Don't throw exception in insert
			a = new Address();
			Address.blacklistedZipCode = "3232";
			a.Id = 13;
			a.Country = "Country";
			a.Line1 = "Line 1";
			a.Zip = "nonnumeric";
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
					Address saved = s.Load<Address>(13L);
					saved.Country = "Country";
					saved.Line1 = "Line 1";
					saved.Zip = "4343";
					saved.State = "NY";
					saved.State = "TOOLONG";
					s.Update(saved);
					t.Commit();
					Assert.Fail("entity should have been validated");
				}
			}
			catch (InvalidStateException e)
			{
				e.GetInvalidValues().Should().Have.Count.EqualTo(1);
			}

			try
			{
				using (s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Address saved = s.Load<Address>(13L);
					saved.Zip = "1234";
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

		[Test, Ignore("Not supported yet without initialization.")]
		public void Components()
		{
			ISession s;
			ITransaction tx;
			s = OpenSession();
			tx = s.BeginTransaction();
			Martian martian = new Martian();
			martian.Id = new MartianPk("Liberal", "Biboudie");
			martian.Address = new MarsAddress("Plus", "cont");
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
	}
}
