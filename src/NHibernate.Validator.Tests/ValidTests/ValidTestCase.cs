using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.ValidTests;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ValidTests
{
	[TestFixture]
	public class ValidTest : BaseValidatorFixture
	{
		[Test]
		public void TestDeepValid()
		{
			ClassValidator formValidator = GetClassValidator(typeof(Form));

			Address a = new Address();
			Member m = new Member();
			m.Address = a;
			Form f = new Form();
			f.Member = m;
			InvalidValue[] values = formValidator.GetInvalidValues(f);
			Assert.AreEqual(1, values.Length);

			m.Address.City = "my city";
			InvalidValue[] values2 = formValidator.GetInvalidValues(f);
			Assert.AreEqual(0, values2.Length);
		}

		[Test]
		public void OneToOneValid()
		{
			ClassValidator vtor = GetClassValidator(typeof(Blog));
			Blog b = new Blog();
			b.Author = new Author();
			InvalidValue[] values = vtor.GetInvalidValues(b);
			Assert.AreEqual(2, values.Length);
		}
	}
}