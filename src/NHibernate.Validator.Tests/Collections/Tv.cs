using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Collections
{
	using System;
	using System.Collections.Generic;

	public class Tv
	{
		[NotNull] 
		public String name;

		[Valid] 
		public IList<Presenter> presenters = new List<Presenter>();

		[Valid] 
		public IDictionary<string, Show> shows = new Dictionary<String, Show>();

		[Valid]
		public IDictionary<Simple, string> validatableInKey;

		[Valid]
		public Movie[] movies;

		[Valid, Size(Max = 1)] public IList<string> dontNeedDeepValidation;
	}

	public class TvDef: ValidationDef<Tv>
	{
		public TvDef()
		{
			Define(x => x.name).NotNullable();
			Define(x => x.presenters).HasValidElements();
			Define(x => x.shows).HasValidElements();
			Define(x => x.validatableInKey).HasValidElements();
			Define(x => x.movies).HasValidElements();
			Define(x => x.dontNeedDeepValidation).HasValidElements().And.MaxSize(1);
		}
	}
}