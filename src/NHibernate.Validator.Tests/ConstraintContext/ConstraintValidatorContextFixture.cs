using System.Linq;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ConstraintContext
{
	[TestFixture]
	public class ConstraintValidatorContextFixture
	{
		#region Setup/Teardown

		[SetUp]
		public void OnSetup()
		{
			c = new ConstraintValidatorContext("SomeProperty", "This is Default Message");
		}

		#endregion

		private ConstraintValidatorContext c;

		public class SampleClass
		{
			public string Prop { get; set; }
		}

		[Test]
		public void ShouldAddMessage()
		{
			c.AddInvalid("Message 1");
			c.AddInvalid("Message 2");

			Assert.AreEqual(3, c.InvalidMessages.Count());
		}

		[Test]
		public void ShouldBeTheCustomMessageAdded()
		{
			c.AddInvalid("Message 1");
			c.DisableDefaultError();

			Assert.AreEqual("Message 1", c.InvalidMessages.ElementAt(0).Message);
			Assert.AreEqual("SomeProperty", c.InvalidMessages.ElementAt(0).PropertyName);
		}

		[Test]
		public void ShouldBeTheCustomMessageAddedWithAnotherProperty()
		{
			c.AddInvalid("Message 1", "AnotherProperty");
			c.DisableDefaultError();

			Assert.AreEqual("Message 1", c.InvalidMessages.ElementAt(0).Message);
			Assert.AreEqual("AnotherProperty", c.InvalidMessages.ElementAt(0).PropertyName);
		}

		[Test]
		public void ShouldBeTheCustomMessageAddedWithAnotherPropertyUsingExpression()
		{
			c.AddInvalid<SampleClass, string>("Message 1", x => x.Prop);
			c.DisableDefaultError();

			Assert.AreEqual("Message 1", c.InvalidMessages.ElementAt(0).Message);
			Assert.AreEqual("Prop", c.InvalidMessages.ElementAt(0).PropertyName);
		}

		[Test]
		public void ShouldDisableDefaultMessage()
		{
			c.AddInvalid("Message 1");
			c.AddInvalid("Message 2");
			c.DisableDefaultError();

			Assert.AreEqual(2, c.InvalidMessages.Count());
		}
	}
}