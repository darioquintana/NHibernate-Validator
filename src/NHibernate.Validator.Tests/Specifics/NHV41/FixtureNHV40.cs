using System.Collections.Generic;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Specifics.NHV41
{
    [TestFixture]
    public class FixtureNHV40
    {
        [Test]
        public void InterpolatingMemberAndSubMembers()
        {
            var c = new Contractor
                        {
                            SubcontractorHourEntries = new List<SubcontractorHourEntry>
                                                           {
                                                               new SubcontractorHourEntry
                                                                   {
                                                                       Contrator = new SubContractor(1),
                                                                       Hour = 9
                                                                   },
                                                               new SubcontractorHourEntry
                                                                   {
                                                                       Contrator = new SubContractor(2),
                                                                       Hour = 8
                                                                   }
                                                           }
                        };

            var vtor = new ValidatorEngine();
            var values = vtor.Validate(c);
            Assert.AreEqual(2, values.Length);
            Assert.AreEqual("The min value in the SubContractor Id: 1 is invalid. Instead was found: 9", values[0].Message);
            Assert.AreEqual("The min value in the SubContractor Id: 2 is invalid. Instead was found: 8", values[1].Message);
        }
    }
}