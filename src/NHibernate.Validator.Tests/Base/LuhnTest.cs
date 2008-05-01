using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Base
{
	[TestFixture]
	public class LuhnTest : BaseValidatorFixture
	{
		[Test]
		public void CreditCard()
		{
			CreditCard card = new CreditCard();
			card.number = "1234567890123456";
			ClassValidator classValidator = GetClassValidator(typeof(CreditCard));
			InvalidValue[] invalidValues = classValidator.GetInvalidValues(card);
			Assert.AreEqual(1, invalidValues.Length);
			card.number = "541234567890125"; //right CC (luhn compliant)
			invalidValues = classValidator.GetInvalidValues(card);
			Assert.AreEqual(0, invalidValues.Length);
			card.ean = "9782266156066"; //right EAN
			invalidValues = classValidator.GetInvalidValues(card);
			Assert.AreEqual(0, invalidValues.Length);
			card.ean = "9782266156067"; //wrong EAN
			invalidValues = classValidator.GetInvalidValues(card);
			Assert.AreEqual(1, invalidValues.Length);
		}
	}
}
