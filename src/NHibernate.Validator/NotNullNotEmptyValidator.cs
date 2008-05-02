using System.Collections;
using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// Validator for any kind of IEnumerable (including string)
	/// </summary>
	public class NotNullNotEmptyValidator : IValidator, IPropertyConstraint
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
			if (!property.IsComposite && !(property.PersistentClass is SingleTableSubclass))
			{
				foreach (Column column in property.ColumnIterator)
					column.IsNullable = false;
			}
		}
	}
}
