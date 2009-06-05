using System.Reflection;
using System.Linq;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Configuration.Loquacious
{
	[TestFixture]
	public class FluentConfigurationFixture
	{
		[Test] 
		public void Minimal()
		{
			var fc = new FluentConfiguration();
			fc.SetMessageInterpolator<MessageInterpolatorStub>()
				.SetDefaultValidatorMode(ValidatorMode.OverrideExternalWithAttribute)
				.IntegrateWithNHibernate.ApplyingDDLConstraints().And.RegisteringListeners();
			var cfg = (INHVConfiguration) fc;
			Assert.That(cfg.Properties["apply_to_ddl"], Is.EqualTo("true"));
			Assert.That(cfg.Properties["autoregister_listeners"], Is.EqualTo("true"));
			Assert.That(cfg.Properties["message_interpolator_class"], Is.EqualTo(typeof(MessageInterpolatorStub).AssemblyQualifiedName));
			Assert.That(cfg.Properties["external_mappings_loader_class"], Is.EqualTo(typeof(FluentMappingLoader).AssemblyQualifiedName));
			Assert.That(cfg.Properties["default_validator_mode"], Is.EqualTo("OverrideExternalWithAttribute".ToLowerInvariant()));

			fc = new FluentConfiguration();
			fc.SetMessageInterpolator<MessageInterpolatorStub>()
				.SetDefaultValidatorMode(ValidatorMode.UseExternal)
				.IntegrateWithNHibernate.AvoidingDDLConstraints().And.AvoidingListenersRegister();
			cfg = fc;
			Assert.That(cfg.Properties["apply_to_ddl"], Is.EqualTo("false"));
			Assert.That(cfg.Properties["autoregister_listeners"], Is.EqualTo("false"));
			Assert.That(cfg.Properties["message_interpolator_class"], Is.EqualTo(typeof(MessageInterpolatorStub).AssemblyQualifiedName));
			Assert.That(cfg.Properties["external_mappings_loader_class"], Is.EqualTo(typeof(FluentMappingLoader).AssemblyQualifiedName));
			Assert.That(cfg.Properties["default_validator_mode"], Is.EqualTo("UseExternal".ToLowerInvariant()));
		}

		[Test] 
		public void RegisterSpecificDefinitions()
		{
			var fc = new FluentConfiguration();
			fc.Register<AddressValidationDef, Address>().Register<BooValidationDef, Boo>();
			var ml = (IMappingLoader) fc;
			Assert.That(ml.GetMappings().Count(), Is.EqualTo(2));
		}

		[Test]
		public void RegisterDefinitions()
		{
			var fc = new FluentConfiguration();
			fc.Register(
				Assembly.GetExecutingAssembly().ValidationDefinitions()
				.Where(x => x.Namespace.Equals("NHibernate.Validator.Tests.Configuration.Loquacious") &&
				(x.Name.StartsWith("Address") || x.Name.StartsWith("Boo"))));
			var ml = (IMappingLoader)fc;
			Assert.That(ml.GetMappings().Count(), Is.EqualTo(2));
		}

		[Test]
		public void CanInitialezeValidatorEngine()
		{
			var fc = new FluentConfiguration();
			fc.SetDefaultValidatorMode(ValidatorMode.UseExternal)
				.Register<AddressValidationDef, Address>().Register<BooValidationDef, Boo>()
				.IntegrateWithNHibernate.AvoidingDDLConstraints().And.AvoidingListenersRegister();

			var ve = new ValidatorEngine();
			ve.Configure(fc);
			Assert.That(ve.GetValidator<Address>(), Is.Not.Null);
			Assert.That(ve.GetValidator<Boo>(), Is.Not.Null);
		}

		public class MessageInterpolatorStub : IMessageInterpolator
		{
			public string Interpolate(string message, object entity, IValidator validator, IMessageInterpolator defaultInterpolator)
			{
				return message;
			}
		}
	}
}