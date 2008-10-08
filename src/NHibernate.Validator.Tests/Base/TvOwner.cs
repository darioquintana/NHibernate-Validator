using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Base
{
	public class TvOwner
	{
		public int id;

		[NotNull, Valid] public Tv tv;
	}
}