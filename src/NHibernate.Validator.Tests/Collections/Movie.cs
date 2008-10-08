using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Collections
{
	public class Movie
	{
		[NotNull] public string Name;
	}
}