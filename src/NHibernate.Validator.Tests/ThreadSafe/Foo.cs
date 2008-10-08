using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.ThreadSafe
{
	public class Foo
	{
		[NotNull]
		public int? Value;
	}
}