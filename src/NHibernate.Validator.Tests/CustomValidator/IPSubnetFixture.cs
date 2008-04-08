using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.CustomValidator
{
	[TestFixture]
	public class SubnetIPFixture : BaseValidatorFixture
	{
		public override ClassValidator GetClassValidator(System.Type type)
		{
			return ClassValidatorFactory.GetValidatorForUseXmlTest(type);
		}

		[Test]
		public void IPIsInSubnetByPrefix()
		{
			Controller controller = new Controller();
			controller.Name = null;
			controller.IP = "192.168.1.1";
			ClassValidator validator = GetClassValidator(typeof(Controller));

			InvalidValue[] res = validator.GetInvalidValues(controller);
			Assert.AreEqual(3, res.Length);

			controller.IP = "192.168.2.1";
			res = validator.GetInvalidValues(controller);
			Assert.AreEqual(2, res.Length);

			controller.Name = "Controller";
			res = validator.GetInvalidValues(controller);
			Assert.AreEqual(0, res.Length);
		}
	}
}
