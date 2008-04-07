namespace NHibernate.Validator
{
	using System;

	/// <summary>
	/// The string has to be a well-formed email address
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(EmailValidator))]
	public class EmailAttribute : Attribute, IHasMessage
	{
		private string message = "{validator.email}";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}