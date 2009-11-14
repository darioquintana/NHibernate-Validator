using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Specifics.NHV71
{
	public class Customer
	{
		public int Id { get; set; }

		[NotNullNotEmpty]
		[Length(10)]
		public string Name { get; set; }

		[Valid]
		public ContactInfo Contact { get; set; }
	}

	public class ContactInfo
	{
		public string Phone { get; set; }

		[Email]
		public string Email { get; set; }
	}
}