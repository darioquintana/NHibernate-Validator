using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// Check that a Date representation apply in the past
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(PastValidator))]
	public class PastAttribute : Attribute, IRuleArgs
	{
		private string message = "{validator.past}";

		public string Message {
			get { return message; }
			set { message = value; }
		}
	}
}