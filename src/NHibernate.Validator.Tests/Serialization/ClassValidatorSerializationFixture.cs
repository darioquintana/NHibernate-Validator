using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.Base;
using NHibernate.Validator.Tests.Integration;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Serialization
{
	[TestFixture]
	public class ClassValidatorSerializationFixture
	{
		[Test]
		public void IsSerializable()
		{
			Assert.That(typeof (ClassValidator), Has.Attribute<SerializableAttribute>());
		}

		[Test]
		public void CanBeSerialized()
		{
			ClassValidator cv = new ClassValidator(typeof(Address));
			Assert.That(cv, Is.BinarySerializable);
		}

		[Test]
		public void WorkAfterDeserialization()
		{
			PrefixMessageInterpolator pmi = new PrefixMessageInterpolator();

			ClassValidator cv = new ClassValidator(typeof(Address),
													new ResourceManager("NHibernate.Validator.Tests.Resource.Messages",
													Assembly.GetExecutingAssembly()), pmi, new CultureInfo("en"),
													 ValidatorMode.UseAttribute);

			RunSerializationTest(cv);

			cv = new ClassValidator(typeof(Address), new ResourceManager("NHibernate.Validator.Tests.Resource.Messages",
													Assembly.GetExecutingAssembly()), pmi, new CultureInfo("en"),
													ValidatorMode.UseXml);
			RunSerializationTest(cv);
		}

		private static void RunSerializationTest(IClassValidator cv)
		{
			Address a = new Address();
			Address.blacklistedZipCode = null;
			a.Country = "Australia";
			a.Zip = "1221341234123";
			a.State = "Vic";
			a.Line1 = "Karbarook Ave";
			a.Id = 3;
			InvalidValue[] validationMessages = cv.GetInvalidValues(a);

			// All work before serialization
			Assert.AreEqual(2, validationMessages.Length); //static field is tested also
			Assert.IsTrue(validationMessages[0].Message.StartsWith("prefix_"));
			Assert.IsTrue(validationMessages[1].Message.StartsWith("prefix_"));

			// Serialize and deserialize
			ClassValidator cvAfter = (ClassValidator)SerializationHelper.Deserialize(SerializationHelper.Serialize(cv));
			validationMessages = cvAfter.GetInvalidValues(a);
			// Now test after
			Assert.AreEqual(2, validationMessages.Length); //static field is tested also
			Assert.IsTrue(validationMessages[0].Message.StartsWith("prefix_"));
			Assert.IsTrue(validationMessages[1].Message.StartsWith("prefix_"));
		}
	}
}
