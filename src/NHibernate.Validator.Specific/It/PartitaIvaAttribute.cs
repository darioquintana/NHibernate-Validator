using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.It
{
	/// <summary>
	/// This expression matches an Italian Partita Iva Code.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(PartitaIvaValidator))]
	public class PartitaIvaAttribute : Attribute, IRuleArgs
	{
		private string message = "Partita IVA incorretta.";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}