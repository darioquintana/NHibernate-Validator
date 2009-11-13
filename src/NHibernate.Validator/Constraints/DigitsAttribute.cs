using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Checks wheter the member is a number having up to <c>IntegerDigits</c>,
	/// and <c>FractionalDigits</c> fractional digits.
	/// <remarks>
	/// <c>FractionalDigits</c> default value: 0.
	/// </remarks> 
	/// <code>
	/// For example:
	/// <example>
	/// - With [Digits(3)] //here FractionalDigits is zero.
	///		- Valid values: 0; 9; 99; 99.0; null; 01;
	///		- Invalid values: 1000; 10.1; "aa"; new object();
	/// 
	/// - With [Digits(2,3)]
	///		- Valid values: 0; 100.100; 99.99; 
	///		- Invalid values: 1000.0; 9.233; 1233; 
	/// 
	/// - With [Digits(0,2)]
	///		- Valid values: 0; 0.1; 0.12;
	///		- Invalid values: 1.12; 0.123
	/// </example>
	/// </code>
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (DigitsValidator))]
	public class DigitsAttribute : EmbeddedRuleArgsAttribute, IRuleArgs
	{
		private string message = "{validator.digits}";
		public DigitsAttribute() {}

		public DigitsAttribute(int integerDigits)
		{
			IntegerDigits = integerDigits;
		}

		public DigitsAttribute(int integerDigits, int fractionalDigits) : this(integerDigits)
		{
			FractionalDigits = fractionalDigits;
		}

		public int IntegerDigits { get; set; }

		public int FractionalDigits { get; set; }

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}