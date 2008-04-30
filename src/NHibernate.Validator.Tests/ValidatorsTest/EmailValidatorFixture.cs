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
			Assert.IsTrue(v.IsValid("emmanuel@hibernate.org"));
			Assert.IsTrue(v.IsValid(""));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsFalse(v.IsValid("emmanuel.hibernate.org"));
			Assert.IsTrue(v.IsValid("emmanuel@hibernate"));
			Assert.IsTrue(v.IsValid("emma-n_uel@hibernate"));
			Assert.IsFalse(v.IsValid("emma nuel@hibernate.org"));
			Assert.IsFalse(v.IsValid("emma(nuel@hibernate.org"));
			Assert.IsFalse(v.IsValid("emmanuel@"));
			Assert.IsTrue(v.IsValid("emma+nuel@hibernate.org"));
			Assert.IsTrue(v.IsValid("emma=nuel@hibernate.org"));
			Assert.IsFalse(v.IsValid("emma\nnuel@hibernate.org"));
			Assert.IsFalse(v.IsValid("emma@nuel@hibernate.org"));
			Assert.IsFalse(v.IsValid("emmanuel@@hibernate.org"));
			Assert.IsFalse(v.IsValid("emmanuel @ hibernate.org"));
			Assert.IsTrue(v.IsValid("emmanuel@[123.12.2.11]"));
			Assert.IsFalse(v.IsValid(".emma@nuel@hibernate.org"));
			Assert.IsFalse(v.IsValid(5)); // check any values different of string
		}
	}
}
