using NHibernate.Validator.Constraints;

namespace MvcNhvDemo.Models
{
	public class Customer : Entity
	{
		[Length(1, 50)]
		public string Name { get; set; }

		[Length(1, 75),Email]
		public string Email { get; set; }
	}
}
