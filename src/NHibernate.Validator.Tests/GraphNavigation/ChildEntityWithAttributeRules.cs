using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.GraphNavigation
{
	public class ChildEntityWithAttributeRules
	{
		[NotNull]
		public string NotNullProperty { get; set; }

		// properties validated by ChildEntityWithAttributeRulesDef
		public bool IsWordMonkeyAllowedInName { get; set; }

		public string Name { get; set;}	
	}
}
