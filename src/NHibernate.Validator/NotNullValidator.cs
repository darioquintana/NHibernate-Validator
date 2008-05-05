using System;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	[Serializable]
	public class NotNullValidator : IValidator, IPropertyConstraint
	{
		public virtual bool IsValid(object value)
		{
			return value != null;
		}

		public void Apply(Property property)
		{
			//single table should not be forced to null
			if (!property.IsComposite && !(property.PersistentClass is SingleTableSubclass))
			{
				foreach (Column column in property.ColumnIterator)
					column.IsNullable = false;
			}
		}
	}
}