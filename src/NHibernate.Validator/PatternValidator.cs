using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class PatternValidator : AbstractValidator<PatternAttribute>
	{
		private Regex regex;

		public override bool IsValid(object value)
		{
			if (value == null) return true;

			if (!(value is string)) return false;

			return regex.IsMatch((string) value);
		}

		public override void Initialize(PatternAttribute parameters)
		{
			PatternAttribute @param =(PatternAttribute)parameters;
			regex = new Regex(@param.Regex,@param.Flags);
		}
	}
}