namespace NHibernate.Validator
{
	using System;
	
	/// <summary>
	/// Check that a Date representation apply in the future
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(FutureValidator))]
	public class FutureAttribute : Attribute, IHasMessage
	{
		private string message = "{validator.future}";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}