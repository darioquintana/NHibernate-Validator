using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests
{
	using System;

	[AttributeUsage(AttributeTargets.Class)]
	[ValidatorClass(typeof(AssertAnimalValidator))]
	public class AssertAnimalAttribute : Attribute, IHasMessage
	{
		private string message = "not an animal";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}

	public class AssertAnimalValidator : IInitializableValidator<AssertAnimalAttribute>
	{
		public bool IsValid(object value)
		{
			return value is Animal;
		}

		public void Initialize(AssertAnimalAttribute parameters)
		{
			
		}
	}
}