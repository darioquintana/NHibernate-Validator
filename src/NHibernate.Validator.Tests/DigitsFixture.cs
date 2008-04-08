using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests
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
			ClassValidator classValidator = GetClassValidator(typeof(Car));
			InvalidValue[] invalidValues = classValidator.GetInvalidValues(car);
			Assert.AreEqual(2, invalidValues.Length);
			car.length = (decimal)1.223; //more than 2
			car.gallons = 10.300; //1 digit really so not invalid
			invalidValues = classValidator.GetInvalidValues(car);
			Assert.AreEqual(1, invalidValues.Length);

			car.length = (decimal)1.200; // 1 digit really so not invalid
			car.gallons = 10.300; //1 digit really so not invalid
			invalidValues = classValidator.GetInvalidValues(car);
			Assert.AreEqual(0, invalidValues.Length);
		}
	}
}
