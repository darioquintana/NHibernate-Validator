using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class PatternValidator : IInitializableValidator<PatternAttribute>
	{
		private Regex regex;

		#region ctor

		/// <summary>
		/// Initialize a Pattern validator
		/// </summary>
		public PatternValidator()
		{
		}

		/// <summary>
		/// Initialize a Pattern validator. Constructor useful for subclasses
		/// </summary>
		/// <param name="regex"></param>
		public PatternValidator(Regex regex)
		{
			this.regex = regex;
		}

		#endregion

		#region IInitializableValidator<PatternAttribute> Members

		public bool IsValid(object value)
		{
			if (value == null) return true;

			if (!(value is string)) return false;

			return regex.IsMatch((string) value);
		}

		public void Initialize(PatternAttribute parameters)
		{
			regex = new Regex(parameters.Regex, parameters.Flags);
		}

		#endregion
	}
}