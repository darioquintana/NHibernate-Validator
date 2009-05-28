using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.ConstraintContext.EntityValidation
{
	public class PasswordValidator2 : IValidator
	{
		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			var info = value as IMembershipInfo;
			if (info == null) throw new InvalidOperationException("The Validator Password can only be applied to implementations of " +
				                                    typeof (IMembershipInfo).FullName);
			
			constraintValidatorContext.DisableDefaultError();

			bool isValid = true;

			if (string.IsNullOrEmpty(info.Username))
			{
				constraintValidatorContext.AddInvalid(Messages.PasswordLength);
				isValid = false;
			}

			if (info.Username != null && info.Password.Contains(info.Username))
			{
				constraintValidatorContext.AddInvalid(Messages.PasswordContentUsername);
				isValid = false;
			}

			if (info.Password.Length < 5)
			{
				constraintValidatorContext.AddInvalid(Messages.PasswordLength);
				isValid = false;
			}

			if (info.Password.Contains("123"))
			{
				constraintValidatorContext.AddInvalid(Messages.PasswordContent);
				isValid = false;
			}

			return isValid;
		}

		#endregion
	}
}