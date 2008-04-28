using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Base
{
	[TestFixture]
	public class CompositionFixture : BaseValidatorFixture
	{
		[Test]
		public void CompositionHasValidationRules()
		{
			ClassValidator vtor = GetClassValidator(typeof(Composition));

			Assert.IsTrue(vtor.HasValidationRules);
		}
	}
}
