using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.ConstraintContext.MemberValidation
{
	public class PasswordValidator : IValidator
	{
		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			constraintValidatorContext.DisableDefaultError();
			bool isValid = true;
			string password = (string)value;

			if(password.Length < 5)
			{
				constraintValidatorContext.AddInvalid(Messages.PasswordLength);
				isValid = false;
			}

			if(password.Contains("123"))
			{
				constraintValidatorContext.AddInvalid(Messages.PasswordContent);
				isValid = false;
			}
			
			return isValid;
		}
	}
}