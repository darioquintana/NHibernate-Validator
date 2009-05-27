using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Configuration
{
	internal class ACustomValidator : IInitializableValidator<ACustomAttribute>
	{
		#region IInitializableValidator<ACustomAttribute> Members

		public void Initialize(ACustomAttribute parameters) {}

		public bool IsValid(object value, IConstraintValidatorContext constraintContext)
		{
			return true;
		}

		#endregion
	}
}