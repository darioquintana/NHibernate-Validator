using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Specifics.NHV62
{
	public class LegsValidator : IValidator
	{
		/// <summary>
		/// does the object/element pass the constraints
		/// </summary>
		/// <param name="value">Object to be validated</param>
		/// <param name="constraintValidatorContext">Context for the validator constraint</param>
		/// <returns>if the instance is valid</returns>
		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			var animal = value as Animal;
			if (animal != null && animal.Name == "Horse" && animal.Legs != 2)
			{
				constraintValidatorContext.DisableDefaultError();
				constraintValidatorContext.AddInvalid<Animal, int>("Legs should be two.", a => a.Legs);

				return false;
			}
			return true;
		}
	}

	[ValidatorClass(typeof (LegsValidator))]
	public class LegsAttribute : Attribute, IRuleArgs
	{
		public string Message { get; set; }
	}

	[Legs(Message = "Legs problem.")]
	public class Animal
	{
		public string Name { get; set; }
		public int Legs { get; set; }
		public int Eyes { get; set; }
	}
}