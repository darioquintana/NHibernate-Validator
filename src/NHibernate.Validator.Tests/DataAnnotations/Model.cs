using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.DataAnnotations
{
	public class A
	{
		[IPAddress]
		public string IpAddress { get; set; }
	}

	[DelegatedValidator(typeof(BValidator), ErrorMessage = "Test")]
	public class B
	{
	}

	class BValidator : IValidator
	{
		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext constraintValidatorContext)
		{
			return false;
		}

		#endregion
	}
}
