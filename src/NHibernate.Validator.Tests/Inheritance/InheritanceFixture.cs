using NHibernate.Validator.Engine;

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
            InvalidValue[] invalidValues = classValidator.GetInvalidValues(dog);
            Assert.AreEqual(3, invalidValues.Length);
            dog.FavoriteBone = "DE";  //failure
            invalidValues = classValidator.GetInvalidValues(dog);
            Assert.AreEqual(3, invalidValues.Length);
        }
    }
}