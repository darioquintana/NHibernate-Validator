using System.Linq;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Engine
{
    [TestFixture]
    public class EngineMocking
    {
        [Test]
        public void CanMockMethods()
        {
            int countNonVirtualMethods = (from m in typeof (ValidatorEngine).GetMethods()
                                          where !m.IsVirtual && m.IsPublic && m.Name != "GetType"
                                          select m).Count();

            Assert.AreEqual(0, countNonVirtualMethods, "Every public method OR property should be virtual for mocking");
        }
    }
}