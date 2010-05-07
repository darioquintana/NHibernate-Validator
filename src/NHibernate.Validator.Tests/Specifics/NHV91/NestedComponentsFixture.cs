using System.Collections;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Specifics.NHV91
{
	[TestFixture]
	public class NestedComponentsFixture : PersistenceTest
	{
		protected override IList Mappings
		{
			get { return new[] {"Specifics.NHV91.Model.hbm.xml"}; }
		}

		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			var configure = new FluentConfiguration();

			configure
				.SetDefaultValidatorMode(ValidatorMode.UseExternal)
				.IntegrateWithNHibernate
				.ApplyingDDLConstraints()
				.And
				.RegisteringListeners();

			var validatorEngine = new ValidatorEngine();
			validatorEngine.Configure(configure);

			configuration.Initialize(validatorEngine);
		}

		[Test]
		public void Should_run()
		{
			//actually the test is in the Configure() method, should run without problems
		}
	}
}