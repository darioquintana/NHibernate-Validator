using System.Text.RegularExpressions;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Pl
{
	public class PostalCodeValidator : IValidator
	{
		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			var check = new Regex(@"^\d{2}-\d{3}$", RegexOptions.Compiled);

			if (value == null)
				return true;

			string postalCode = value.ToString();

			if (postalCode == string.Empty)
				return true;

			return check.IsMatch(postalCode);
		}

		#endregion
	}
}