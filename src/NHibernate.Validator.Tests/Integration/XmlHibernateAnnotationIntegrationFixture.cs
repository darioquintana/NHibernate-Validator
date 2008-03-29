using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NHibernate.Validator.Cfg;
using NHibernate.Util;

namespace NHibernate.Validator.Tests.Integration
{
	public class XmlHibernateAnnotationIntegrationFixture : HibernateAnnotationIntegrationFixture
	{
		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			cfg.SetProperty(Environment.MessageInterpolatorClass,
							   typeof(PrefixMessageInterpolator).AssemblyQualifiedName);
			cfg.SetProperty(Environment.ValidatorMode, "usexml");
			ValidatorInitializer.Initialize(cfg);
		}	
	}
}
