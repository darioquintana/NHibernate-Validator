using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.GraphNavigation
{
	public class ChildEntityWithAttributeRulesDef : ValidationDef<ChildEntityWithAttributeRules>
	{
		/// <summary>
		/// Exposed for testing
		/// </summary>
		public const string Message = "Cannot use the word monkey";

		public ChildEntityWithAttributeRulesDef()
		{
			ValidateInstance.By(ValidateChild).WithMessage(Message);
		}

		private static bool ValidateChild(ChildEntityWithAttributeRules entity, IConstraintValidatorContext context)
		{
			if (entity.IsWordMonkeyAllowedInName)
			{
				return true;
			}
			else if(entity.Name == null)
			{
				return true;
			}
			else
			{
				return !entity.Name.ToLowerInvariant().Contains("monkey");
			}	
		}
	}
}
