using System.Collections.Generic;
using System.Linq;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.GraphNavigation
{
	[TestFixture]
	public class Fixture
	{
		[Test,Ignore("Implementing...")]
		//[Test]
		public void GraphNavigationDeterminism()
		{
			// build the test object graph
			var user = new User("John", "Doe");

			var address1 = new Address(null, "11122", "Stockholm");
			address1.SetInhabitant(user);

			var address2 = new Address("Kungsgatan 5", "11122", "Stockholm");
			address2.SetInhabitant(user);

			user.AddAddress(address1);
			user.AddAddress(address2);

			var order = new Order(1);
			order.ShippingAddress = address1;
			order.BillingAddress = address2;
			order.Customer = user;

			var line1 = new OrderLine(order, 42);
			var line2 = new OrderLine(order, 101);
			order.AddOrderLine(line1);
			order.AddOrderLine(line2);

			var vtor = new ValidatorEngine();

			InvalidValue[] constraintViolations = vtor.Validate(order);
			Assert.AreEqual(3, constraintViolations.Length, "Wrong number of constraints");

			var expectedErrorMessages = new List<string>();
			expectedErrorMessages.Add("shippingAddress.addressline1");
			expectedErrorMessages.Add("customer.addresses[0].addressline1");
			expectedErrorMessages.Add("billingAddress.inhabitant.addresses[0].addressline1");

			foreach (InvalidValue violation in constraintViolations)
			{
				if (expectedErrorMessages.Contains(violation.PropertyPath))
				{
					expectedErrorMessages.Remove(violation.PropertyPath);
				}
			}

			Assert.IsTrue(expectedErrorMessages.Count == 0, "All error messages should have occured once");
		}

		[Test, Ignore("Implementing...")]
		public void testNoEndlessLoop()
		{
			var john = new User("John", null);
			john.Knows(john);

			var validator = new ValidatorEngine();

			InvalidValue[] constraintViolations = validator.Validate(john);
			Assert.AreEqual(constraintViolations.Length, 1, "Wrong number of constraints");
			Assert.AreEqual("lastname", constraintViolations.ElementAt(0).PropertyName);
			//TestUtil.assertConstraintViolation(constraintViolations.iterator().next(), "may not be null", User.class, null, "lastName" );


			var jane = new User("Jane", "Doe");
			jane.Knows(john);
			john.Knows(jane);

			constraintViolations = validator.Validate(john);
			Assert.AreEqual(constraintViolations.Length, 1, "Wrong number of constraints");
			Assert.AreEqual("lastname", constraintViolations.ElementAt(0).PropertyName);

			//constraintViolations = validator.validate( jane );
			//assertEquals( constraintViolations.size(), 1, "Wrong number of constraints" );
			//TestUtil.assertConstraintViolation(
			//        constraintViolations.iterator().next(), "may not be null", User.class, null, "knowsUser[0].lastName"
			//);

			//john.setLastName( "Doe" );
			//constraintViolations = validator.validate( john );
			//assertEquals( constraintViolations.size(), 0, "Wrong number of constraints" );
		}
	}
}