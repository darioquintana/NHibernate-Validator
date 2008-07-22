using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// This expression matches an IBAN Code (International Bank Account Number)
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(IBANValidator))]
	public class IBANAttribute : Attribute, IRuleArgs
	{
		private string message = "{validator.iban}";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}