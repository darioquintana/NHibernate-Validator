using NHibernate.Validator.Cfg;

namespace NHibernate.Validator.Tests.Integration
{
	public class IntegrationOwnEngineFixture : HibernateAnnotationIntegrationFixture
	{
		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			Environment.SharedEngineProvider = null;

			ValidatorInitializer.Initialize(configuration);
		}

		protected override void EnsureSharedEngine()
		{
		}
	}
}
