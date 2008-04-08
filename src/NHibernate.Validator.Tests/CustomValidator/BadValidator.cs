using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.CustomValidator
{
	/// <summary>
	/// Wrong validator, must thrown an exception when used.
	/// Must implement IHasMessage interface.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(BadValidator))]
	public class BadValidatorAttribute : Attribute
	{
	}

	public class BadValidator : Validator<BadValidatorAttribute>
	{
		public override bool IsValid(object value)
		{
			//Always no valid, then the message can be used, and then an exception must be thorwn.
			return false;
		}

		public override void Initialize(BadValidatorAttribute parameters)
		{
			
		}
	}
}
