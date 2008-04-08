using System;

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