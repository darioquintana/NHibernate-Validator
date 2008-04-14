using System.Text.RegularExpressions;

namespace NHibernate.Validator.Demo.Winforms.Model
{
	public class PhoneValidator : PatternValidator
	{
		public PhoneValidator()
			: base(new Regex(@"^[2-9]\d{2}-\d{3}-\d{4}$"))
		{
		}

		public bool IsValid(object value)
		{
			return base.IsValid(value);
		}
	}
}