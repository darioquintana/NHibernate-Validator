using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// The Attribute element has to represent an EAN-13 or UPC-A
	/// which aims to check for user mistake, not actual number validity!
	/// http://en.wikipedia.org/wiki/European_Article_Number
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (EANValidator))]
	public class EANAttribute : Attribute, IRuleArgs
	{
		private string message = "{validator.ean}";

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}