using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Demo.IoC.Windsor.MyValidators
{
	/// <summary>
	/// Validate that a name begins with Upper Case.
	/// <remarks>
	/// Note that this Validator can be registered into a IoC container
	/// with a singleton lifestyle (or lifecycle). It's stateless.
	/// </remarks>
	/// </summary>
	public class PersonNameValidator : IValidator
	{
		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext context)
		{
			context.DisableDefaultError();

			string name = value as string;
            if(name == null) return true;

			if(name.Length < 2)
				context.AddInvalid("The name should have at least 2 letters.");

			if(name.Length > 0)
			{
				char firstLetter = name[0];

				bool isLower = firstLetter.ToString() == firstLetter.ToString().ToLower();
				context.AddInvalid("The name should begin with Upper Case.");
				if(isLower) return false;
			}

			return true;
		}

		#endregion
	}
}