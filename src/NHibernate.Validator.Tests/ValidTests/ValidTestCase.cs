using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.ValidTests
{
	[TestFixture]
	public class ValidTest : BaseValidatorFixture
	{
		[Test]
		public void TestDeepValid()
		{
			IClassValidator formValidator = GetClassValidator(typeof(Form));

			Address a = new Address();
			Member m = new Member();
			m.Address = a;
			Form f = new Form();
			f.Member = m;
			formValidator.GetInvalidValues(f).Should().Have.Count.EqualTo(1);

			m.Address.City = "my city";
			formValidator.GetInvalidValues(f).Should().Be.Empty();
		}

		[Test]
		public void OneToOneValid()
		{
			IClassValidator vtor = GetClassValidator(typeof(Blog));
			Blog b = new Blog();
			b.Author = new Author();
			vtor.GetInvalidValues(b).Should().Have.Count.EqualTo(2);
		}
	}
}