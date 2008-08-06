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

	public class BadValidator : IValidator
	{
		public bool IsValid(object value)
		{
			//Always no valid, then the message can be used, and then an exception must be thorwn.
			return false;
		}
	}

	/// <summary>
	/// Wrong validator, must thrown an Validation exception when used.
	/// Must have a message like:
	/// <code>
	/// <example>
	/// private string message = "{validator.MyValidatorMessage}"; 
	/// 
	/// OR
	/// 
	/// private string message = "Error on property Foo"; 
	/// 
	/// </example>
	/// </code>
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(BadValidator))]
	public class BadValidatorMessageIsNullAttribute : Attribute, IRuleArgs
	{
		private string message;

		public string Message
		{
			set { message = value; }
			get { return message; }
		}
	}
}
