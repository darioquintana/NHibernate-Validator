using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Pl
{
	/// <summary>
	/// This expression matches the Polish postal code
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (PostalCodeValidator))]
	public class PostalCodeAttribute : Attribute, IRuleArgs
	{
		private string message = "Nieprawidlowy kod pocztowy.";

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}