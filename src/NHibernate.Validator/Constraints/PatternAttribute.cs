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
	[ValidatorClass(typeof (PatternValidator))]
	public class PatternAttribute : Attribute, IRuleArgs
	{
		private RegexOptions flags = RegexOptions.Compiled;
		private string message = "{validator.pattern}";

		public PatternAttribute() {}

		public PatternAttribute(string regex)
		{
			Regex = regex;
		}

		public PatternAttribute(string regex, RegexOptions flags) : this(regex)
		{
			this.flags = flags;
		}

		public PatternAttribute(string regex, RegexOptions flags, string message) : this(regex, flags)
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
	}
}