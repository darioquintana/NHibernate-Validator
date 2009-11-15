using NHibernate.Validator.Engine;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Inheritance
{
    using NUnit.Framework;

    [TestFixture]
    public class InheritanceFixture : BaseValidatorFixture
    {
        [Test]
        public void TestInh()
        {
            IClassValidator classValidator = GetClassValidator(typeof(Dog));
            Dog dog = new Dog();
            classValidator.GetInvalidValues(dog).Should().Have.Count.EqualTo(3);
            dog.FavoriteBone = "DE";  //failure
						classValidator.GetInvalidValues(dog).Should().Have.Count.EqualTo(3);
        }
    }
}