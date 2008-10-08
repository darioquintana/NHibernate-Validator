using System;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.ValidatorsTest
{
	public class FooDate
	{
		[Future]
		public DateTime Future;

		[Past]
		public DateTime Past;
	}
}