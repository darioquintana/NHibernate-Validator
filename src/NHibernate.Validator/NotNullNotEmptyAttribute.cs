using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// Not empty and not null constraint
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	[ValidatorClass(typeof(NotNullNotEmptyValidator))]
	public class NotNullNotEmptyAttribute : Attribute, IRuleArgs
	{
		private string message = "{validator.notNullNotEmpty}";
		#region IHasMessage Members

		/// <summary>
		/// The message to be sent to the user if it is not valid
		/// </summary>
		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}
