using System;
using NHibernate.Validator.Demo.Ev.Validators;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Demo.Ev.Model
{
	/// <summary>
	/// Attribute in this namespace to write less in the xml configuration
	/// </summary>
	[ValidatorClass(typeof(ValidRangeValidator))]
	public class ValidRangeAttribute : Attribute, IRuleArgs
	{
		public ValidRangeAttribute()
		{
			Message = "Invalid range, the end date should be major than the start date";
		}

		public string Message { get; set; }
	}
}