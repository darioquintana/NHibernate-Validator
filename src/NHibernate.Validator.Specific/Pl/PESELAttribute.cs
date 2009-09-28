using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Pl
{
	/// <summary>
	/// This expression matches the Polish Identification Number (PESEL)
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (PESELValidator))]
	public class PESELAttribute : Attribute, IRuleArgs
	{
		private string message = "Nieprawidlowy numer PESEL.";

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}