using System;

namespace NHibernate.Validator.Tests.Base
{
	public class User
	{
		[Email] public String email;

		[NotNull] public String name;
	}
}