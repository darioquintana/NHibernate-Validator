using System.Text.RegularExpressions;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class PatternValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			PatternValidator v = new PatternValidator();
			v.Initialize(new PatternAttribute("^4[0-9]{12}(?:[0-9]{3})?$", RegexOptions.Singleline, "mess"));
			Assert.IsTrue(v.IsValid("4408041234567893"));
			Assert.IsTrue(v.IsValid(4408041234567893UL));
			Assert.IsFalse(v.IsValid(""));
			Assert.IsTrue(v.IsValid(null));

			v.Initialize(new PatternAttribute(@"(19|20)\d\d([- /.])(0[1-9]|1[012])\2(0[1-9]|[12][0-9]|3[01])", RegexOptions.Singleline));
			Assert.IsTrue(v.IsValid("1999-01-01"));
			Assert.IsFalse(v.IsValid("1999/01-01"));
		}
	}
}
