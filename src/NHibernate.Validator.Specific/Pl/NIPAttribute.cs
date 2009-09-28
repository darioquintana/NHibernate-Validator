using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Pl
{
	/// <summary>
	/// This expression matches the Polish Tax Number (NIP)
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (NIPValidator))]
	public class NIPAttribute : Attribute, IRuleArgs
	{
		private string message = "Nieprawidlowy numer NIP.";

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}