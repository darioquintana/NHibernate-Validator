using System;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.CustomValidator
{
	[TestFixture]
	public class BadValidatorFixture : BaseValidatorFixture
	{
		public class Foo
		{
			[BadValidator]
			private string SomeField;
		}


		[Test,ExpectedException(typeof(ArgumentException))]
		public void ExceptionMustBeThrown()
		{
			IClassValidator vtor = new ClassValidator(typeof (Foo));
			
			Foo f = new Foo();

			vtor.GetInvalidValues(f);
		}
	}
}
