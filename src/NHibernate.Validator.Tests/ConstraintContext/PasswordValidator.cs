using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.ConstraintContext
{
	public class PasswordValidator : IValidator
	{
		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			bool isValid = true;
			string password = (string)value;

			if(password.Length < 5)
			{
				constraintValidatorContext.DisableDefaultError();
				constraintValidatorContext.AddInvalid(Messages.PasswordLength);
				isValid = false;
			}

			if(password.Contains("123"))
			{
				constraintValidatorContext.DisableDefaultError();
				constraintValidatorContext.AddInvalid(Messages.PasswordContent);
				isValid = false;
			}
			
			return isValid;
		}
	}
}