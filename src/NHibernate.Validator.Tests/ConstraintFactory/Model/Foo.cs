using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.ConstraintFactory.Model
{
	public class Foo
	{
		[NotNullNotEmpty]
		public string Name { get; set; }

		[NotNullNotEmpty]
		[LengthAttribute(Min = 10, Max = 300)]
		public string Description { get; set; }
	}
}