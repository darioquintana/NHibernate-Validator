using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Check if string is a number.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(IsNumericValidator))]
	public class IsNumericAttribute : Attribute, IRuleArgs
	{
		public IsNumericAttribute()
		{
			Message = "{validator.numeric}";
		}

		#region Implementation of IRuleArgs

		public string Message { get; set; }

		#endregion
	}
}