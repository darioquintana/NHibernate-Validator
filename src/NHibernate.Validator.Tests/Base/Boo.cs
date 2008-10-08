using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Base
{
	public class Boo
	{
		[NotNullNotEmpty]
		public string field;
	}
}