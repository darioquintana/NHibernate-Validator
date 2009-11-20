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
	public class PatternAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator
	{
		private RegexOptions flags = RegexOptions.Compiled;
		private Regex regex;
		private string message = "{validator.pattern}";

		public PatternAttribute() { }

		public PatternAttribute(string regex)
		{
			Regex = regex;
		}

		public PatternAttribute(string regex, RegexOptions flags)
			: this(regex)
		{
			this.flags = flags;
		}

		public PatternAttribute(string regex, RegexOptions flags, string message)
			: this(regex, flags)
		{
			this.message = message;
		}

		public string Regex { get; set; }

		public RegexOptions Flags
		{
			get { return flags; }
			set { flags = value; }
		}

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion

		#region IInitializableValidator<PatternAttribute> Members

		public virtual bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			return value == null || GetRegex().IsMatch(value.ToString());
		}

		protected Regex GetRegex()
		{
			return regex ?? (regex = new Regex(Regex, Flags));
		}

		#endregion
	}
}