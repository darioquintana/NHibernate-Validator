using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.CustomValidator
{
	[TestFixture]
	public class ValidatorClassFixture 
	{
		[Test]
		public void OnlyAttributes()
		{
			ValidatorClassAttribute vc1 =
				new ValidatorClassAttribute(
					"NHibernate.Validator.Tests.CustomValidator.IsOneValidator, NHibernate.Validator.Tests");

			Assert.IsNotNull(vc1.Value);

			ValidatorClassAttribute vc2 =
				new ValidatorClassAttribute(typeof(IsOneValidator));

			Assert.IsNotNull(vc1.Value);

			Assert.AreEqual(vc1.Value,vc2.Value);

		}

		[Test]
		public void IntegrationWithValidation()
		{
			ValidatorEngine ve = new ValidatorEngine();
			ve.AssertValid(new Foo(1));

			Assert.IsFalse(ve.IsValid(new Foo(3)));
		}
		
		public class Foo
		{
			public Foo(int bar)
			{
				Bar = bar;
			}

			[IsOne]
			public int Bar;
		}

	}
}
