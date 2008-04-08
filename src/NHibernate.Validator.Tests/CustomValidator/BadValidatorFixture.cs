using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;
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


		[Test,ExpectedException(typeof(HibernateValidatorException))]
		public void ExceptionMustBeThrown()
		{
			IClassValidator vtor = new ClassValidator(typeof (Foo));
			
			Foo f = new Foo();

			vtor.GetInvalidValues(f);
		}
	}
}
