using System.Collections.Generic;

namespace NHibernate.Validator.Engine
{
	public interface IClassValidatorImplementor
	{
		IEnumerable<InvalidValue> GetInvalidValues(object entity, HashSet<object> circularityState, ICollection<object> activeTags);
		IDictionary<System.Type, IClassValidator> ChildClassValidators { get;}
	}
}