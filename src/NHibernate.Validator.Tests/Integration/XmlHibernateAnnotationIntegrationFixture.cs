using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Integration
{
	public class XmlHibernateAnnotationIntegrationFixture : HibernateAnnotationIntegrationFixture
	{
		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			NHVConfiguration nhvc = new NHVConfiguration();
			nhvc.Properties[Environment.ValidatorMode] = "usexml";
			nhvc.Properties[Environment.MessageInterpolatorClass] = typeof(PrefixMessageInterpolator).AssemblyQualifiedName;
			ConfigurationInjecterSectionHandler.SetCfgToInject(nhvc);
			ValidatorInitializer.Initialize(configuration);
		}

		protected override void OnTestFixtureTearDown()
		{
			ConfigurationInjecterSectionHandler.ResetCfgToInject();
		}
	}
}
