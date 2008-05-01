using System.Collections;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class NotEmptyValidator : IValidator, IPropertyConstraint
	{
		public bool IsValid(object value)
		{
			IEnumerable ev = value as IEnumerable;
			if (ev != null)
			{
				return ev.GetEnumerator().MoveNext();
			}

			return false;
		}

		public void Apply(Property property)
		{
			//single table should not be forced to null
			if (property is SingleTableSubclass) return;

			if (!property.IsComposite)
				foreach (Column column in property.ColumnIterator)
					column.IsNullable = false;
		}
	}
}