using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Configuration.Loquacious
{
	[TestFixture]
	public class IntegrationFixture
	{
		[Test]
		public void LoadMappingsSpecific()
		{
			var nhvc = new XmlConfiguration();
			nhvc.Properties[Environment.ValidatorMode] = "useExternal";
			nhvc.Properties[Environment.MappingLoaderClass] = "NHibernate.Validator.Cfg.Loquacious.FluentMappingLoader, NHibernate.Validator";
			nhvc.Mappings.Add(new MappingConfiguration("NHibernate.Validator.Tests",
			                                           "NHibernate.Validator.Tests.Configuration.Loquacious.AddressValidationDef"));
			nhvc.Mappings.Add(new MappingConfiguration("NHibernate.Validator.Tests",
																								 "NHibernate.Validator.Tests.Configuration.Loquacious.BooValidationDef"));
			var ve = new ValidatorEngine();
			ve.Configure(nhvc);
			var a = new Address {Country = string.Empty};
			var b = new Boo();
			Assert.That(ve.IsValid(a));
			Assert.That(!ve.IsValid(b));
			a.Country = "bigThan5Chars";
			Assert.That(!ve.IsValid(a));
			b.field = "whatever";
			Assert.That(ve.IsValid(b));
		}
	}
}