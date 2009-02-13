using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Cfg.Loquacious.Impl
{
	public class FinalRuleArgsOptions: IRuleArgsOptions
	{
		private readonly IRuleArgs constraintAttribute;

		public FinalRuleArgsOptions(IRuleArgs constraintAttribute)
		{
			if (constraintAttribute == null)
			{
				throw new ArgumentNullException("constraintAttribute");
			}
			this.constraintAttribute = constraintAttribute;
		}

		#region Implementation of IRuleArgsOptions

		public void WithMessage(string message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			constraintAttribute.Message = message;
		}

		#endregion
	}
}