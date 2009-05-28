using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.ConstraintContext
{
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
}