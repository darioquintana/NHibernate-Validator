using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(DigitsValidator))]
	public class DigitsAttribute : Attribute, IRuleArgs
	{
		public DigitsAttribute()
		{

		}

		public DigitsAttribute(int integerDigits)
		{
			this.integerDigits = integerDigits;
		}

		public DigitsAttribute(int integerDigits, int fractionalDigits) : this(integerDigits)
		{
			this.fractionalDigits = fractionalDigits;
		}

		private int integerDigits;

		public int IntegerDigits
		{
			get { return integerDigits; }
			set { integerDigits = value; }
		}

		private int fractionalDigits = 0;

		public int FractionalDigits
		{
			get { return fractionalDigits; }
			set { fractionalDigits = value; }
		}


		private string message = "{validator.digits}";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}