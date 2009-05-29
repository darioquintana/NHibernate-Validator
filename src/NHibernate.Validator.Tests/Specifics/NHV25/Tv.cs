using System;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Specifics.NHV25
{
	public class Tv
	{
		[NotNull] public String name;
        
		[Valid] public IDictionary<int, Show> shows = new Dictionary<int, Show>();

		[Valid] public IDictionary<TestEnum, Show> showse = new Dictionary<TestEnum, Show>();
	}

	public enum TestEnum
	{
		uno = 1,
		due = 2,
		tre = 3
	}
}