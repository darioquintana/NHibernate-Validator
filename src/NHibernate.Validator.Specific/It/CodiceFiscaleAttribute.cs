using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.It
{
	/// <summary>
	/// This expression matches the Italian Fiscal Code.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(CodiceFiscaleValidator))]
	public class CodiceFiscaleAttribute : Attribute, IRuleArgs
	{
		private string message = "Codica fiscale FORMALMENTE incorretto.";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}