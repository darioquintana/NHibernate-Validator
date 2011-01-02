using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Demo.Model
{
	public class PhoneValidator : IInitializableValidator<PhoneAttribute>
	{
		private Regex regex;

		public bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			if (value == null) return true;
			return regex.IsMatch(value.ToString());
		}

		public void Initialize(PhoneAttribute parameters)
		{
			regex = new Regex(@"^[2-9]\d{2}-\d{3}-\d{4}$");
		}
	}
}