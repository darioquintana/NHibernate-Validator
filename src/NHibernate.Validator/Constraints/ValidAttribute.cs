using System;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Enables recursive validation of an associated object
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class ValidAttribute : EmbeddedRuleArgsAttribute { }
}