using System.Collections;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Integration
{
	[TestFixture]
	public class ValidatingProxyFixture : PersistenceTest
	{
		protected override IList Mappings
		{
			get { return new[] { "Integration.SimpleWithRelation.hbm.xml" }; }
		}

		[Test, Ignore("Not fixed yet")]
		public void ValidateInitializedProxyAtFirstLevel()
		{
			var validatorConf = new FluentConfiguration();
			validatorConf.SetDefaultValidatorMode(ValidatorMode.UseExternal);
			
			var vDefSimple = new ValidationDef<SimpleWithRelation>();
			vDefSimple.Define(s => s.Name).MatchWith("OK");

			var engine = new ValidatorEngine();
			engine.Configure(validatorConf);

			object savedId;
			// fill DB
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				savedId = s.Save(new SimpleWithRelation { Name = "OK" });
				tx.Commit();
			}

			using (ISession s = OpenSession())
			{
				var proxy = s.Load<SimpleWithRelation>(savedId);
				NHibernateUtil.Initialize(proxy);
				Assert.That(engine.IsValid(proxy));
			}

			CleanDb();
		}

		[Test, Ignore("Not fixed yet")]
		public void ValidateNotInitializedProxyAtFirstLevel()
		{
			var validatorConf = new FluentConfiguration();
			validatorConf.SetDefaultValidatorMode(ValidatorMode.UseExternal);

			var vDefSimple = new ValidationDef<SimpleWithRelation>();
			vDefSimple.Define(s => s.Name).MatchWith("OK");

			var engine = new ValidatorEngine();
			engine.Configure(validatorConf);

			object savedId;
			// fill DB
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				savedId = s.Save(new SimpleWithRelation { Name = "OK" });
				tx.Commit();
			}

			using (ISession s = OpenSession())
			{
				var proxy = s.Load<SimpleWithRelation>(savedId);
				Assert.That(engine.IsValid(proxy));
				Assert.That(!NHibernateUtil.IsInitialized(proxy), "should not initialize the proxy");
			}

			CleanDb();
		}

		private void CleanDb()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.CreateQuery("delete from Relation").ExecuteUpdate();
				s.CreateQuery("delete from SimpleWithRelation").ExecuteUpdate();
				tx.Commit();
			}
		}
	}
}