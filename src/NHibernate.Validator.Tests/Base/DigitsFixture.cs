using System;
using NUnit.Framework;
using NHibernate.Validator.Engine;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Base
{
	[TestFixture]
	public class DigitsFixture : BaseValidatorFixture
	{
		[Test]
		public void TestDigits()
		{
			Car car = new Car();
			car.name = "350Z";
			car.insurances = new String[] { "random" };
			car.length = (decimal)10.2;
			car.gallons = 100.3;
			IClassValidator classValidator = GetClassValidator(typeof(Car));
			classValidator.GetInvalidValues(car).Should().Have.Count.EqualTo(2);
			car.length = (decimal)1.223; //more than 2
			car.gallons = 10.300; //1 digit really so not invalid
			classValidator.GetInvalidValues(car).Should().Have.Count.EqualTo(1);

			car.length = (decimal)1.200; // 1 digit really so not invalid
			car.gallons = 10.300; //1 digit really so not invalid
			classValidator.GetInvalidValues(car).Should().Be.Empty();
		}
	}
}