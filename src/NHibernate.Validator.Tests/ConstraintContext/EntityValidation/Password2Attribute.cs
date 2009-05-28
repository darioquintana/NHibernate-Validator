using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.ConstraintContext.EntityValidation
{
	[ValidatorClass(typeof(PasswordValidator2))]
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	internal sealed class Password2Attribute : Attribute, IRuleArgs
	{
		public Password2Attribute()
		{
			Message = "{Password2}";
		}

		public string Message { get; set; }
	}
}