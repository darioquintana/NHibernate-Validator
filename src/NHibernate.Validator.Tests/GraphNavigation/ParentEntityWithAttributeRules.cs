using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.GraphNavigation
{
	public class ParentEntityWithAttributeRules
	{
		[Valid]
		public ChildEntityWithAttributeRules Child { get; set; }
	}
}
