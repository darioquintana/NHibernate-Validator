using System;
using NHibernate.Validator.Demo.Model;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Demo.Model
{
	/// <summary>
	/// This expression matches a hyphen separated US phone number, 
	/// of the form ANN-NNN-NNNN, where A is between 2 and 9 and N 
	/// is between 0 and 9.
	/// 
	/// Matches: 800-555-5555 | 333-444-5555 | 212-666-1234
	/// No Matches: 000-000-0000 | 123-456-7890 | 2126661234
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(PhoneValidator))]
	public class PhoneAttribute : Attribute, IRuleArgs
	{
		private string message = string.Empty;

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}