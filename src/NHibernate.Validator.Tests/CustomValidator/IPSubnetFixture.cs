using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.CustomValidator
{
	[TestFixture]
	public class SubnetIPFixture : BaseValidatorFixture
	{
		public override IClassValidator GetClassValidator(System.Type type)
		{
			return UtilValidatorFactory.GetValidatorForUseExternalTest(type);
		}

		[Test]
		public void IPIsInSubnetByPrefix()
		{
			Controller controller = new Controller();
			controller.Name = null;
			controller.IP = "192.168.1.1";
			IClassValidator validator = GetClassValidator(typeof(Controller));

			validator.GetInvalidValues(controller).Should().Have.Count.EqualTo(2);

			controller.IP = "192.168.2.1";
			validator.GetInvalidValues(controller).Should().Have.Count.EqualTo(1);

			controller.Name = "Controller";
			validator.GetInvalidValues(controller).Should().Be.Empty();
		}
	}
}
