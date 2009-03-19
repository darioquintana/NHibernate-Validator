using System.Collections.Generic;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Specifics.NHV41
{
    public class Contractor
    {
        [Valid]
        public IList<SubcontractorHourEntry> SubcontractorHourEntries { get; set; }
    }

    public class SubcontractorHourEntry
    {
        public SubContractor Contrator { get; set; }

        [Min(Value = 10, Message = "The min value in the SubContractor Id: ${Contrator.Id} is invalid. Instead was found: ${Hour}")]
        public int Hour { get; set; }
    }

    public class SubContractor
    {
        public int Id { get; private set; }

        public SubContractor(int i)
        {
            Id = i;
        }
    }

    public class SubContractors
    {
    }
}