using System.Linq;
using System.Text.RegularExpressions;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class PatternValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			var v = new PatternAttribute("^4[0-9]{12}(?:[0-9]{3})?$", RegexOptions.Singleline, "mess");
			Assert.IsTrue(v.IsValid("4408041234567893", null));
			Assert.IsTrue(v.IsValid(4408041234567893UL, null));
			Assert.IsFalse(v.IsValid("", null));
			Assert.IsTrue(v.IsValid(null, null));

			v = new PatternAttribute(@"(19|20)\d\d([- /.])(0[1-9]|1[012])\2(0[1-9]|[12][0-9]|3[01])", RegexOptions.Singleline,
									 null);
			Assert.IsTrue(v.IsValid("1999-01-01", null));
			Assert.IsFalse(v.IsValid("1999/01-01", null));
		}

		[Test]
		public void AllowMoreThanOne()
		{
			var e = new WithRegEx { Value = "aaa" };
			var ca = new ClassValidator(typeof(WithRegEx));
			var iv = ca.GetInvalidValues(e).ToArray();
			Assert.That(() => iv = ca.GetInvalidValues(e).ToArray(), Throws.Nothing);
		}
	}

	public class WithRegEx
	{
		[Pattern(@"\d")]
		[Pattern(@"[0-9]")]
		public string Value { get; set; }
	}
}
