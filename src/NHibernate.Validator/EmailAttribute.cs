using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// The string has to be a well-formed email address
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (EmailValidator))]
	public class EmailAttribute : Attribute, IRuleArgs
	{
		private string message = "{validator.email}";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}