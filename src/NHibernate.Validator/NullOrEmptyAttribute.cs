using System;

namespace NHibernate.Validator
{
	/// <summary>
	/// Not empty and not null constraint
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	[ValidatorClass(typeof(NotNullOrEmptyValidator))]
	public class NotNullOrEmptyAttribute : Attribute, IHasMessage
	{
		private string message = "{validator.notNullOrEmpty}";
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
