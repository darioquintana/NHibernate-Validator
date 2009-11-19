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
		[NonSerialized]
		private Regex regex;
		private string message = "{validator.pattern}";

		public PatternAttribute()
		{
			Flags = RegexOptions.Compiled;
		}

		public PatternAttribute(string regex)
		{
			Flags = RegexOptions.Compiled;
			Regex = regex;
		}

		public PatternAttribute(string regex, RegexOptions flags) : this(regex)
		{
			Flags = flags;
		}

		public PatternAttribute(string regex, RegexOptions flags, string message) : this(regex, flags)
		{
			Message = message;
		}

		public string Regex { get; set; }

		public RegexOptions Flags { get; set; }

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion

		#region Implementation of IValidator

		public virtual bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			if (value == null)
			{
				return true;
			}

			return GetRegEx().IsMatch(value.ToString());
		}

		#endregion

		private Regex GetRegEx()
		{
			return regex ?? (regex = new Regex(Regex, Flags));
		}
	}
}