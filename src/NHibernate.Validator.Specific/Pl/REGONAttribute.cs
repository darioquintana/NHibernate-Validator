using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Pl
{
	/// <summary>
	/// This expression matches the Polish National Business Register Number (REGON)
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (REGONValidator))]
	public class REGONAttribute : Attribute, IRuleArgs
	{
		private string message = "Nieprawidlowy numer REGON.";

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}