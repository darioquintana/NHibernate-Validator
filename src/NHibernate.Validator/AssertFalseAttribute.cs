using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// The annotated element has to be false
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(AssertFalseValidator))]
	public class AssertFalseAttribute : Attribute, IRuleArgs
	{
		private string message = "{validator.assertFalse}";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}
