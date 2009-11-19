using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.ConstraintFactory.Model
{
	public class MyValidator: IValidator
	{
		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			return true;
		}
	}
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(MyValidator))]
	public class MyAttribute : Attribute, IRuleArgs
	{
		public MyAttribute()
		{
			Message = "something";
		}

		public string Message { get; set; }
	}
}