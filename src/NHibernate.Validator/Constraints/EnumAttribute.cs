using System;
using System.Collections;
using System.Linq;

using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Value assigned to enum property must be a valid enum value (Enum.IsDefined(..) or valid flags combination)
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class EnumAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator, IPropertyConstraint
	{
		private string message = "{validator.enum}";

		public EnumAttribute()
		{
		}

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion

		#region IValidator Members

		public bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			if (value == null)
				return true;

			var tp = value.GetType();

			return tp.GetCustomAttributes(typeof (FlagsAttribute), false).Length > 0 
				? IsFlagsValid(tp, (long)Convert.ChangeType(value, typeof(long))) 
				: Enum.IsDefined(tp, value);
		}


		//Next method is taken there
		//http://www.codeproject.com/Tips/194804/See-if-a-Flags-enum-is-valid
		private static bool IsFlagsValid(System.Type enumType, long value)
		{
			if (enumType.IsEnum && enumType.IsDefined(typeof(FlagsAttribute), false))
			{
				long compositeValues = Enum.GetValues(enumType)
											.Cast<object>()
											.Aggregate<object, long>(0, (current, flag) => current | Convert.ToInt64(flag));

				return (~compositeValues & value) == 0;
			}
			else
			{
				return false;
			}
		}

		#endregion

		#region IPropertyConstraint Members

		public void Apply(Property property)
		{
			IEnumerator ie = property.ColumnIterator.GetEnumerator();
			ie.MoveNext();
			var col = (Column)ie.Current;

			if (property.Type.ReturnedClass.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0)
				return;

			var valuesSeparated = Enum.GetValues(property.Type.ReturnedClass)
										.Cast<object>()
										.Aggregate(String.Empty, (current, val) => current + Convert.ChangeType(val, typeof(long)) + ", ")
										.TrimEnd(',',' ');

			col.CheckConstraint = col.Name + " in (" + valuesSeparated + ")";
		}

		#endregion
	}
}
