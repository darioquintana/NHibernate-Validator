using System.Collections.Generic;
using Iesi.Collections;

namespace NHibernate.Validator.Engine
{
	public interface IClassValidatorImplementor
	{
		InvalidValue[] GetInvalidValues(object entity, ISet circularityState);
		IDictionary<System.Type, IClassValidator> ChildClassValidators { get;}
	}
}