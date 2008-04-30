using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Event;

namespace NHibernate.Validator.Tests.Integration
{
	public class XmlHibernateAnnotationIntegrationFixture : HibernateAnnotationIntegrationFixture
	{
		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			// The ValidatorInitializer and the ValidateEventListener share the same engine

			// Initialize the SharedEngine
			Environment.SharedEngineProvider = new NHibernateSharedEngineProvider();
			ValidatorEngine ve = Environment.SharedEngineProvider.GetEngine();
			ve.Clear();
			NHVConfiguration nhvc = new NHVConfiguration();
			nhvc.Properties[Environment.ApplyToDDL] = "true";
			nhvc.Properties[Environment.AutoregisterListeners] = "true";
			nhvc.Properties[Environment.ValidatorMode] = "usexml";
			nhvc.Properties[Environment.MessageInterpolatorClass] = typeof(PrefixMessageInterpolator).AssemblyQualifiedName;
			ve.Configure(nhvc);

			ValidatorInitializer.Initialize(configuration);
		}
	}
}
