using System;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;

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

		public void WithTags(params object[] tags)
		{
			if(tags == null)
			{
				return;
			}
			var tagableRule = constraintAttribute as ITagableRule;
			if(tagableRule == null)
			{
				throw new ValidatorConfigurationException(string.Format("The constraint {0} does not supports tags.",constraintAttribute.GetType()));
			}
			Array.ForEach(tags, tag => tagableRule.TagCollection.Add(tag));
		}

		#endregion
	}
}