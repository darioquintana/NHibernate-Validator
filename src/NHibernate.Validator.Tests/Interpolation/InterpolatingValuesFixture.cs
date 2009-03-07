using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Interpolation
{
	[TestFixture]
	public class InterpolatingValuesFixture
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
			        		                           				Hour = 10
			        		                           			}
			        		                           	}
			};

			var vtor = new ValidatorEngine();
			Assert.IsFalse(vtor.IsValid(c));
			var values = vtor.Validate(c);
			Assert.AreEqual(1, values.Length);
			Assert.AreEqual("The min value in the SubContractor Id: 1 is invalid. Instead was found: 9", values[0].Message);
		}
	}

	public class Contractor
	{
		[Valid]
		public IList<SubcontractorHourEntry> SubcontractorHourEntries { get; set; }
	}

	public class SubcontractorHourEntry
	{
		public SubContractor Contrator { get; set; }

		[Min(Value = 10,Message = "The min value in the SubContractor Id: ${Contrator.Id} is invalid. Instead was found: ${Hour}")]
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