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
			fortest = new NHibernateSharedEngineProvider();
			Environment.SharedEngineProvider = fortest;
			ValidatorEngine ve = Environment.SharedEngineProvider.GetEngine();
			ve.Clear();
			NHVConfiguration nhvc = new NHVConfiguration();
			nhvc.Properties[Environment.ApplyToDDL] = "true";
			nhvc.Properties[Environment.AutoregisterListeners] = "true";
			nhvc.Properties[Environment.ValidatorMode] = "useExternal";
			nhvc.Properties[Environment.MessageInterpolatorClass] = typeof(PrefixMessageInterpolator).AssemblyQualifiedName;
			ve.Configure(nhvc);
			ve.IsValid(new AnyClass());// add the element to engine for test

			ValidatorInitializer.Initialize(configuration);
		}
	}
}
