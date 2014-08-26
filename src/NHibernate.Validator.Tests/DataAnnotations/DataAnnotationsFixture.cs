using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.DataAnnotations
{
	[TestFixture]
	public class DataAnnotationsFixture : BaseValidatorFixture
	{
		[Test]
		public void ShouldValidatePropertyWithDataAnnotationsValidator()
		{
			var instance = new A { IpAddress = "aaa.bbb.ccc" };
			var context = new ValidationContext(instance, null, new Dictionary<object, object>());
			var results = new List<ValidationResult>();

			System.ComponentModel.DataAnnotations.Validator.TryValidateObject(instance, context, results, true);

			var invalidValue = results.Single();
			invalidValue.ErrorMessage.Should().Be.EqualTo("{validator.ipaddress}");
		}

		[Test]
		public void ShouldValidateClassWithDataAnnotationsValidator()
		{
			var instance = new B();
			var context = new ValidationContext(instance, null, new Dictionary<object, object>());
			var results = new List<ValidationResult>();

			System.ComponentModel.DataAnnotations.Validator.TryValidateObject(instance, context, results);

			var invalidValue = results.Single();
			invalidValue.ErrorMessage.Should().Be.EqualTo("Test");
		}
	}
}