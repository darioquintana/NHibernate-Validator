using NHibernate.Validator.Util;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Attributes
{
	[TestFixture]
	public class AttributesFixture
	{
		[Test]
		public void AttributeCanBeMultiplied()
		{
			PatternAttribute patternAttribute = new PatternAttribute();
			Assert.AreEqual(true, (AttributeUtils.AttributeAllowsMultiple(patternAttribute)));
		}

		[Test]
		public void AttributeCannotBeMultiplied()
		{
			LengthAttribute lenghtAttribute = new LengthAttribute();
			Assert.AreEqual(false, (AttributeUtils.AttributeAllowsMultiple(lenghtAttribute)));
		}
	}
}
