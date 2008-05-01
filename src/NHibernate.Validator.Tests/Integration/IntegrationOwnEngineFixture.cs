using NHibernate.Validator.Cfg;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Integration
{
	public class IntegrationOwnEngineFixture : HibernateAnnotationIntegrationFixture
	{
		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			Environment.SharedEngineProvider = null;

			ValidatorInitializer.Initialize(configuration);
		}

		public override void EnsureSharedEngine()
		{
			Assert.IsNull(Environment.SharedEngineProvider);
		}
	}
}
