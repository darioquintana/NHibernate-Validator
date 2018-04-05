using System;
using System.Globalization;
using System.Linq;
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
			TestsContext.AssumeSystemTypeIsSerializable();
			ClassValidator cv = new ClassValidator(typeof(Address));
			NHAssert.IsSerializable(cv);
		}

		[Test]
		public void WorkAfterDeserialization()
		{
			TestsContext.AssumeSystemTypeIsSerializable();
			PrefixMessageInterpolator pmi = new PrefixMessageInterpolator();

			ClassValidator cv = new ClassValidator(typeof(Address),
													new ResourceManager("NHibernate.Validator.Tests.Resource.Messages",
													Assembly.GetExecutingAssembly()), pmi, new CultureInfo("en"),
													 ValidatorMode.UseAttribute);

			RunSerializationTest(cv);

			cv = new ClassValidator(typeof(Address), new ResourceManager("NHibernate.Validator.Tests.Resource.Messages",
													Assembly.GetExecutingAssembly()), pmi, new CultureInfo("en"),
													ValidatorMode.UseExternal);
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
			var validationMessages = cv.GetInvalidValues(a).ToArray();

			// All work before serialization
			Assert.That(validationMessages, Has.Length.EqualTo(2)); //static field is tested also
			Assert.That(validationMessages, Has.All.Message.StartsWith("prefix_"));

			// Serialize and deserialize
			ClassValidator cvAfter = (ClassValidator)SerializationHelper.Deserialize(SerializationHelper.Serialize(cv));
			validationMessages = cvAfter.GetInvalidValues(a).ToArray();
			// Now test after
			Assert.That(validationMessages, Has.Length.EqualTo(2)); //static field is tested also
			Assert.That(validationMessages, Has.All.Message.StartsWith("prefix_"));
		}
	}
}
