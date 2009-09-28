using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Specifics.NHV62
{
	[TestFixture]
	public class Fixture
	{
		[Test]
		public void InterpolatingMemberAndSubMembers()
		{
			var animal = new Animal {Legs = 1, Name = "Horse"};

			var vtor = new ValidatorEngine();
			var values = vtor.Validate(animal);
			Assert.AreEqual(1, values.Length);
			//The legs validator add a error message for the property Legs.
			Assert.AreEqual("Legs", values[0].PropertyName);
		}
	}
}