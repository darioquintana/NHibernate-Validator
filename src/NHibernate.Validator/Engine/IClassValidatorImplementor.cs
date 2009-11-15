using System.Collections.Generic;
using Iesi.Collections;

namespace NHibernate.Validator.Engine
{
	public interface IClassValidatorImplementor
	{
		IEnumerable<InvalidValue> GetInvalidValues(object entity, ISet circularityState, ICollection<object> activeTags);
		IDictionary<System.Type, IClassValidator> ChildClassValidators { get;}
	}
}