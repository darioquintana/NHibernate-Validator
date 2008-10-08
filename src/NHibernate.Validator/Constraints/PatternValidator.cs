using System;
using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class PatternValidator : IInitializableValidator<PatternAttribute>
	{
		private Regex regex;

		#region IInitializableValidator<PatternAttribute> Members

		public virtual bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}

			return regex.IsMatch(value.ToString());
		}

		public virtual void Initialize(PatternAttribute parameters)
		{
			regex = new Regex(parameters.Regex, parameters.Flags);
		}

		#endregion
	}
}