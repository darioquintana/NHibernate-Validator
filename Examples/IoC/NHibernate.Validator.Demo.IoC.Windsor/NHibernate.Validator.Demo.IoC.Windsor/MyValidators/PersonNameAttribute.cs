using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Demo.IoC.Windsor.MyValidators
{
	[ValidatorClass(typeof (PersonNameValidator))]
	public class PersonNameAttribute : Attribute, IRuleArgs
	{
		public PersonNameAttribute()
		{
			Message = "{PersonName.Message}";
		}

		public string Message { get; set; }
	}
}