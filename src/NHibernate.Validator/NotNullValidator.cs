using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class NotNullValidator : IValidator, IPropertyConstraint
	{
		public bool IsValid(object value)
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