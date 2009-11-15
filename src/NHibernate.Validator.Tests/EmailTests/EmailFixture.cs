using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.EmailTests
{
	[TestFixture]
	public class EmailFixture : BaseValidatorFixture
	{
		private IClassValidator userValidator;

		[Test]
		public void testEmail()
		{
			userValidator = GetClassValidator(typeof(User));
			
			isRightEmail("emmanuel@hibernate.org");
			isRightEmail("");
			isRightEmail(null);
			isWrongEmail("emmanuel.hibernate.org");
			isRightEmail("emmanuel@hibernate");
			isRightEmail("emma-n_uel@hibernate");
			isWrongEmail("emma nuel@hibernate.org");
			isWrongEmail("emma(nuel@hibernate.org");
			isWrongEmail("emmanuel@");
			isRightEmail("emma+nuel@hibernate.org");
			isRightEmail("emma=nuel@hibernate.org");
			isWrongEmail("emma\nnuel@hibernate.org");
			isWrongEmail("emma@nuel@hibernate.org");
			isWrongEmail("emmanuel@@hibernate.org");
			isWrongEmail("emmanuel @ hibernate.org");
			isRightEmail("emmanuel@[123.12.2.11]");
			isWrongEmail(".emma@nuel@hibernate.org");
		}

		private void isRightEmail(string email)
		{
			userValidator.GetPotentialInvalidValues("email", email).Should("Wrong email").Be.Empty();
		}

		private void isWrongEmail(string email)
		{
			userValidator.GetPotentialInvalidValues("email", email).Should("Right email").Not.Be.Empty();
		}
	}
}