using System.Collections.Generic;
using System.Linq;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.GraphNavigation
{
	[TestFixture,Ignore("Implementing...")]
	public class Fixture
	{
		[Test]
		public void testGraphNavigationDeterminism()
		{
			// build the test object graph
			var user = new User("John", "Doe");

			var address1 = new Address(null, "11122", "Stockholm");
			address1.setInhabitant(user);

			var address2 = new Address("Kungsgatan 5", "11122", "Stockholm");
			address2.setInhabitant(user);

			user.addAddress(address1);
			user.addAddress(address2);

			var order = new Order(1);
			order.setShippingAddress(address1);
			order.setBillingAddress(address2);
			order.setCustomer(user);

			var line1 = new OrderLine(order, 42);
			var line2 = new OrderLine(order, 101);
			order.addOrderLine(line1);
			order.addOrderLine(line2);

			var vtor = new ValidatorEngine();

			InvalidValue[] constraintViolations = vtor.Validate(order);
			Assert.AreEqual(constraintViolations.Length, 3, "Wrong number of constraints");

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

		[Test]
		public void testNoEndlessLoop()
		{
			var john = new User("John", null);
			john.knows(john);

			var validator = new ValidatorEngine();

			InvalidValue[] constraintViolations = validator.Validate(john);
			Assert.AreEqual(constraintViolations.Length, 1, "Wrong number of constraints");
			Assert.AreEqual("lastname", constraintViolations.ElementAt(0).PropertyName);
			//TestUtil.assertConstraintViolation(constraintViolations.iterator().next(), "may not be null", User.class, null, "lastName" );


			var jane = new User("Jane", "Doe");
			jane.knows(john);
			john.knows(jane);

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