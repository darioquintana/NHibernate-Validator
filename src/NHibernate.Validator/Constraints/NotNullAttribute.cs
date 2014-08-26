using System;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	/// <summary>
	/// Ensure the member to be not null.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class NotNullAttribute : EmbeddedRuleArgsAttribute, IPropertyConstraint
	{
		public NotNullAttribute()
		{
			this.ErrorMessage = "{validator.notNull}";
		}

		#region IPropertyConstraint Members

		public void Apply(Property property)
		{
			//single table should not be forced to null
			if (!property.IsComposite && !(property.PersistentClass is SingleTableSubclass))
			{
				foreach (Column column in property.ColumnIterator)
				{
					column.IsNullable = false;
				}
			}
		}

		#endregion

		#region IValidator Members

		public override bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			return value != null;
		}

		#endregion
	}
}