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
}