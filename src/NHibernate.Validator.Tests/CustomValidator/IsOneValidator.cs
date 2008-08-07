using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.CustomValidator
{

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass("NHibernate.Validator.Tests.CustomValidator.IsOneValidator, NHibernate.Validator.Tests")]
	public class IsOneAttribute : Attribute, IRuleArgs
	{
		private string message = "some message";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}


	public class IsOneValidator : IValidator
	{
		public bool IsValid(object value)
		{
			return (int) value == 1;
		}
	}
}