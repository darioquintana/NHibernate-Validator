using System;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.ConstraintContext
{
	[TestFixture]
	public class ConstraintContextFixture
	{
		[Test]
		public void ShouldDisableTheDefaultMessageAndAddAnothers()
		{
			var vtor = new ValidatorEngine();
			var mi = new MembershipInfo
			                    	{
			                    		Username = "MyName",
			                    		Password = "123X"
			                    	};

			var invalidValues = vtor.Validate(mi);

			Assert.AreEqual(2, invalidValues.Length);
		}
	}


	public class MembershipInfo
	{
		[NotNull]
		public string Username;

		[Password(Message = "Invalid password")]
		public string Password;
	}

	[ValidatorClass(typeof(PasswordValidator))]
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	internal sealed class PasswordAttribute : Attribute, IRuleArgs
	{
		public PasswordAttribute()
		{
			Message = "{Password}";
		}

		public string Message { get; set; }
	}

	public class PasswordValidator : IValidator
	{
		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			bool isValid = true;
			string password = (string)value;

			if(password.Length < 5)
			{
				constraintValidatorContext.DisableDefaultError();
				constraintValidatorContext.AddInvalid("The password has should be larger than 5");
				isValid = false;
			}

			if(password.Contains("123"))
			{
				constraintValidatorContext.DisableDefaultError();
				constraintValidatorContext.AddInvalid("The password can't have the '123' phrase");
				isValid = false;
			}
			
			return isValid;
		}
	}
}
