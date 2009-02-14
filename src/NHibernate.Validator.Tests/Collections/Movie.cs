using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Collections
{
	public class Movie
	{
		[NotNull] public string Name;
	}

	public class MovieDef: ValidationDef<Movie>
	{
		public MovieDef()
		{
			Define(x => x.Name).NotNullable();
		}
	}
}