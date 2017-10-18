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

		public class Foo2
		{
			[BadValidatorMessageIsNull]
			private string SomeField;
		}


		[Test]
		public void ExceptionMustBeThrown()
		{
			Assert.That(() => new ClassValidator(typeof(Foo)), Throws.TypeOf<HibernateValidatorException>());
		}

		/// <summary>
		/// Expected an HibernateValidatorException instead of a NullReferenceException.
		/// </summary>
		[Test]
		public void MessageNull()
		{
			Assert.That(() => new ClassValidator(typeof(Foo2)), Throws.TypeOf<HibernateValidatorException>());
		}
	}
}
