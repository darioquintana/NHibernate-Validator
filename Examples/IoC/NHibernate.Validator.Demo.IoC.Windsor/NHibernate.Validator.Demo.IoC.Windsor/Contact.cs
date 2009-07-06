using NHibernate.Validator.Constraints;
using NHibernate.Validator.Demo.IoC.Windsor.MyValidators;

namespace NHibernate.Validator.Demo.IoC.Windsor
{
	public class Contact
	{
		[NotNullNotEmpty, PersonName]
		public string FirstName { get; set; }

		[NotNullNotEmpty, PersonName]
		public string LastName { get; set; }

		[NotNullNotEmpty]
		public string Address { get; set; }

		[Length(Max = 300)]
		public string Description { get; set; }
	}
}