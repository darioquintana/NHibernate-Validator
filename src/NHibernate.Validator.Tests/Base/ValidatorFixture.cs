using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Base
{
	[TestFixture]
	public class ValidatorFixture : BaseValidatorFixture
	{
		public static readonly string ESCAPING_EL = "(escaping {el})";

		[Test]
		public void AddressValid()
		{
			Address a = new Address();
			Address.blacklistedZipCode = null;
			a.Country = "Australia";
			a.Zip = "1221341234123";
			a.State = "Vic";
			a.Line1 = "Karbarook Ave";
			a.Id = 3;
			IClassValidator classValidator = GetClassValidator(typeof(Address), new ResourceManager("NHibernate.Validator.Tests.Resource.Messages", Assembly.GetExecutingAssembly()), new CultureInfo("en"));
			var validationMessages = classValidator.GetInvalidValues(a);
			if (AllowStaticFields)
			{
				validationMessages.Should().Have.Count.EqualTo(2); //static field is tested also				
			}
			else
			{
				validationMessages.Should().Have.Count.EqualTo(1);
			}
			Address.blacklistedZipCode = "323232";
			a.Zip = null;
			a.State = "Victoria";
			validationMessages = classValidator.GetInvalidValues(a);
			validationMessages.Should().Have.Count.EqualTo(2);
			//validationMessages = classValidator.GetInvalidValues(a, "zip");
			//Assert.AreEqual(1, validationMessages.Length);
			a.Zip = "3181";
			a.State = "NSW";
			validationMessages = classValidator.GetInvalidValues(a);
			validationMessages.Should().Be.Empty();
			a.Country = null;
			validationMessages = classValidator.GetInvalidValues(a);
			validationMessages.Should().Have.Count.EqualTo(1);
			a.InternalValid = false;
			validationMessages = classValidator.GetInvalidValues(a);
			validationMessages.Should().Have.Count.EqualTo(2);
			a.InternalValid = true;
			a.Country = "France";
			a.floor = 4000;
			validationMessages = classValidator.GetInvalidValues(a);
			validationMessages.Should().Have.Count.EqualTo(1);
			string expectedMessage = string.Format("Floor cannot {0} be lower that -2 and greater than 50 {1}", ESCAPING_EL, ESCAPING_EL);
			var invalidValue = validationMessages.First();
			if (TestMessages)
			{
				Assert.AreEqual(expectedMessage, invalidValue.Message);
			}
			Assert.AreEqual(typeof (Address), invalidValue.EntityType);
			Assert.IsTrue(ReferenceEquals(a, invalidValue.Entity));
			Assert.AreEqual(4000, invalidValue.Value);
			if (TestMessages)
			{
				Assert.AreEqual("floor" + "[" + expectedMessage + "]", invalidValue.ToString());
			}
		}

		[Test]
		public void Circularity()
		{
			Brother emmanuel = new Brother();
			emmanuel.Name = "Emmanuel";
			Address.blacklistedZipCode = "666";
			Address address = new Address();
			address.InternalValid = true;
			address.Country = "France";
			address.Id = 3;
			address.Line1 = "Rue des rosiers";
			address.State = "NYC";
			address.Zip = "33333";
			address.floor = 4;
			emmanuel.Address = address;
			Brother christophe = new Brother();
			christophe.Name = "Christophe";
			christophe.Address = address;
			emmanuel.YoungerBrother = christophe;
			christophe.Elder = emmanuel;
			IClassValidator classValidator = GetClassValidator(typeof(Brother));
			var invalidValues = classValidator.GetInvalidValues(emmanuel);
			invalidValues.Should().Be.Empty();
			christophe.Name = null;
			invalidValues = classValidator.GetInvalidValues(emmanuel);
			invalidValues.Should("Name cannot be null").Not.Be.Empty();
			Assert.AreEqual(emmanuel, invalidValues.First().RootEntity);
			Assert.AreEqual("YoungerBrother.Name", invalidValues.First().PropertyPath);
			christophe.Name = "Christophe";
			address = new Address();
			address.InternalValid = true;
			address.Country = "France";
			address.Id = 4;
			address.Line1 = "Rue des plantes";
			address.State = "NYC";
			address.Zip = "33333";
			address.floor = -100;
			christophe.Address = address;
			invalidValues = classValidator.GetInvalidValues(emmanuel);
			invalidValues.Should("Floor cannot be less than -2").Not.Be.Empty();
		}

		[Test]
		public void EntityValidator()
		{
			Suricato s = new Suricato();
			IClassValidator vtor = GetClassValidator(typeof(Suricato));

			Assert.IsTrue(vtor.HasValidationRules);
			var invalidValues = vtor.GetInvalidValues(s);
			invalidValues.Should().Not.Be.Empty();
			invalidValues.Single().Message.Should().Be.EqualTo("is not an animal");
		}

		/// <summary>
		/// Test duplicates annotations.
		/// Hibernate.Validator has diferent behavior: Aggregate Annotations
		/// <example>
		/// example for the field 'info'.
		/// </example>
		/// <code>
		/// [Pattern(Regex = "^[A-Z0-9-]+$", Message = "must contain alphabetical characters only")]
		///	[Pattern(Regex = "^....-....-....$", Message = "must match ....-....-....")]
		/// private string info;
		/// </code> 
		/// </summary>
		[Test]
		public void DuplicateAnnotations()
		{
			CarEngine eng = new CarEngine();
			eng.HorsePower = 23;
			eng.SerialNumber = "23-43###4";
			IClassValidator classValidator = GetClassValidator(typeof(CarEngine));
			var invalidValues = classValidator.GetInvalidValues(eng);
			invalidValues.Should().Have.Count.EqualTo(2);

			//This cannot be tested, the order is random
			//Assert.AreEqual("must contain alphabetical characters only", invalidValues[0].Message);
			//Assert.AreEqual("must match ....-....-....", invalidValues[1].Message);
			//Instead of that, I do this:
			invalidValues.Select(iv => iv.Message).Should()
				.Contain("must contain alphabetical characters only")
				.And
				.Contain("must match ....-....-....");

			eng.SerialNumber = "1234-5678-9012";
			classValidator.GetInvalidValues(eng).Should().Be.Empty();
		}

		[Test]
		public void FieldIsNotNullNotEmpty()
		{
			Boo boo = new Boo();
			boo.field = null;

			IClassValidator validator = GetClassValidator(typeof(Boo));
			var invalids = validator.GetInvalidValues(boo);
			invalids.Should("null value cannot be valid").Not.Be.Empty();

			boo.field = string.Empty;
			invalids = validator.GetInvalidValues(boo);
			invalids.Should("empty value cannot be valid").Not.Be.Empty();
		}

		protected virtual bool AllowStaticFields
		{
			get { return true; }
		}

		protected virtual bool TestMessages
		{
			get { return true; }
		}
	}
}