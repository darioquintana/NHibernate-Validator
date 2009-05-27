using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	[TestFixture]
	public class EmailValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			EmailValidator v = new EmailValidator();
			Assert.IsTrue(v.IsValid("emmanuel@hibernate.org", null));
			Assert.IsTrue(v.IsValid("", null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsFalse(v.IsValid("emmanuel.hibernate.org", null));
			Assert.IsTrue(v.IsValid("emmanuel@hibernate", null));
			Assert.IsTrue(v.IsValid("emma-n_uel@hibernate", null));
			Assert.IsFalse(v.IsValid("emma nuel@hibernate.org", null));
			Assert.IsFalse(v.IsValid("emma(nuel@hibernate.org", null));
			Assert.IsFalse(v.IsValid("emmanuel@", null));
			Assert.IsTrue(v.IsValid("emma+nuel@hibernate.org", null));
			Assert.IsTrue(v.IsValid("emma=nuel@hibernate.org", null));
			Assert.IsFalse(v.IsValid("emma;nuel@hibernate.org", null));
			Assert.IsFalse(v.IsValid("emma(;nuel@hibernate.org", null));
			Assert.IsFalse(v.IsValid("emma\nnuel@hibernate.org", null));
			Assert.IsFalse(v.IsValid("emma@nuel@hibernate.org", null));
			Assert.IsFalse(v.IsValid("emmanuel@@hibernate.org", null));
			Assert.IsFalse(v.IsValid("emmanuel @ hibernate.org", null));
			Assert.IsTrue(v.IsValid("emmanuel@[123.12.2.11]", null));
			Assert.IsFalse(v.IsValid(".emma@nuel@hibernate.org", null));
			Assert.IsFalse(v.IsValid(5, null)); // check any values different of string
		}
	}
}
