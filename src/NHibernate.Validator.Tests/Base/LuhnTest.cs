using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Base
{
	[TestFixture]
	public class LuhnTest : BaseValidatorFixture
	{
		[Test]
		public void GivingValidState_NoInvalidValues()
		{
			var card = new CreditCard {Number = "541234567890125", Ean = "9782266156066"};
			var classValidator = GetClassValidator(typeof(CreditCard));

			classValidator.GetInvalidValues(card).Should().Be.Empty();
		}

		[Test]
		public void GivingInvalidState_HasInvalidValues()
		{
			var card = new CreditCard {Number = "1234567890123456", Ean = "9782266156067"};
			var classValidator = GetClassValidator(typeof(CreditCard));

			classValidator.GetInvalidValues(card).Should().Have.Count.EqualTo(2);
		}
	}
}
