using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// Check that a Date representation apply in the future
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(FutureValidator))]
	public class FutureAttribute : Attribute, IRuleArgs
	{
		// TODO : Add tolerance
		private string message = "{validator.future}";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}