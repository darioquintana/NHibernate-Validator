using System;
using NHibernate.Validator.Engine;
using System.Globalization;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class IsNumericValidator : IValidator
	{
		#region Implementation of IValidator

		public bool IsValid(object value)
		{
			return value == null || (value is string && IsNumeric(value));
		}

		#endregion
		private static bool IsNumeric(object Expression)
		{
			double retNum;

			return double.TryParse(Convert.ToString(Expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
		}
	}
}