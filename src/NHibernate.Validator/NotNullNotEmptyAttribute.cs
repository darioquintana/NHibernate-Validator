using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// Ensure a IEnumerable (including string) to be not null and not empty.
	/// <code>
	/// <example>
	/// Valid values: "abc"; new int[] {1}; new List&lt;int>(new int[] { 1 });
	/// Invalid values: null; ""; 123; new int[0]; new List&lt;int>();
	/// </example>
	/// </code>
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	[ValidatorClass(typeof(NotNullNotEmptyValidator))]
	public class NotNullNotEmptyAttribute : Attribute, IRuleArgs
	{
		private string message = "{validator.notNullNotEmpty}";

		/// <summary>
		/// The message to be sent to the user if it is not valid
		/// </summary>
		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}
