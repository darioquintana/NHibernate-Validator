using NHibernate.Validator.Constraints;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class AssertFalseFixture
	{
		[Test]
		public void IsValid()
		{
			var v = new AssertFalseAttribute();
			Assert.IsTrue(v.IsValid(false, null));
			Assert.IsFalse(v.IsValid(null, null));
			Assert.IsFalse(v.IsValid(true, null));
			Assert.IsFalse(v.IsValid(new object(), null));
		}
	}
}
