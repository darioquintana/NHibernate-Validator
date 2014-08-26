using System;
using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// The annotated element must follow the regex pattern
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	public class PatternAttribute : EmbeddedRuleArgsAttribute
	{
		public PatternAttribute() : this(string.Empty, RegexOptions.Compiled, "{validator.pattern}")
		{
		}

		public PatternAttribute(string regex) : this(regex, RegexOptions.Compiled, "{validator.pattern}")
		{
		}

		public PatternAttribute(string regex, RegexOptions flags) : this(regex, flags, "{validator.pattern}")
		{
		}

		public PatternAttribute(string regex, RegexOptions flags, string message) : this(regex, flags)
		{
			this.Regex = regex;
			this.Flags = flags;
			this.ErrorMessage = message;
		}

		public string Regex { get; set; }

		public RegexOptions Flags
		{
			get;
			set;
		}

		#region IInitializableValidator<PatternAttribute> Members

		public override bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			return value == null || GetRegex().IsMatch(value.ToString());
		}

		protected Regex GetRegex()
		{
			return new Regex(this.Regex, this.Flags);
		}

		#endregion
	}
}