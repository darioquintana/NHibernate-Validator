using System;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.Base;
using NHibernate.Validator.Tests.Integration;
using NUnit.Framework;
using Environment=System.Environment;

namespace NHibernate.Validator.Tests.Serialization
{
	[TestFixture]
	public class ValidatorEngineSerializationFixture
	{
		[SetUp]
		public void OnSetup()
		{
			Cfg.Environment.ConstraintValidatorFactory = null;
		}

		[Test]
		public void IsSerializable()
		{
			Assert.That(typeof(ValidatorEngine), Has.Attribute<SerializableAttribute>());
		}

		[Test]
		public void CanBeSerialized()
		{
			ValidatorEngine ve = new ValidatorEngine();
			Assert.That(ve, Is.BinarySerializable);
		}

		[Test]
		public void WorkAfterDeserialization()
		{
			ValidatorEngine ve = new ValidatorEngine();
			XmlConfiguration nhvc = new XmlConfiguration();
			
			nhvc.Properties[Cfg.Environment.ValidatorMode] = "UseAttribute";
			nhvc.Properties[Cfg.Environment.MessageInterpolatorClass] = typeof(PrefixMessageInterpolator).AssemblyQualifiedName;
			ve.Configure(nhvc);

			RunSerializationTest(ve);

			ve = new ValidatorEngine();
			nhvc = new XmlConfiguration();
			nhvc.Properties[Cfg.Environment.ValidatorMode] = "UseExternal";
			nhvc.Properties[Cfg.Environment.MessageInterpolatorClass] = typeof(PrefixMessageInterpolator).AssemblyQualifiedName;
			ve.Configure(nhvc);

			RunSerializationTest(ve);
		}

		private static void RunSerializationTest(ValidatorEngine ve)
		{
			Address a = new Address();
			Address.blacklistedZipCode = null;
			a.Country = "Australia";
			a.Zip = "1221341234123";
			a.State = "Vic";
			a.Line1 = "Karbarook Ave";
			a.Id = 3;
			InvalidValue[] validationMessages = ve.Validate(a);

			// All work before serialization
			Assert.AreEqual(2, validationMessages.Length); //static field is tested also
			Assert.IsTrue(validationMessages[0].Message.StartsWith("prefix_"));
			Assert.IsTrue(validationMessages[1].Message.StartsWith("prefix_"));

			// Serialize and deserialize
			ValidatorEngine cvAfter = (ValidatorEngine)SerializationHelper.Deserialize(SerializationHelper.Serialize(ve));
			validationMessages = cvAfter.Validate(a);
			// Now test after
			Assert.AreEqual(2, validationMessages.Length); //static field is tested also
			Assert.IsTrue(validationMessages[0].Message.StartsWith("prefix_"));
			Assert.IsTrue(validationMessages[1].Message.StartsWith("prefix_"));
		}
	}
}
