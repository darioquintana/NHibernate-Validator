using System;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class NotNullValidator : IValidator, IPropertyConstraint
	{
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

		public virtual bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			return value != null;
		}

		#endregion
	}
}