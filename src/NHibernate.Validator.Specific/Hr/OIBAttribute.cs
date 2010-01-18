using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Hr
{
	/// <summary>
	/// This expression matches the Croatian Personal Identification Number (OIB - Osobni Identifikacijski Broj).
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (OIBValidator))]
	public class OIBAttribute : Attribute, IRuleArgs
	{
		private string message = "OIB nije ispravan.";

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}