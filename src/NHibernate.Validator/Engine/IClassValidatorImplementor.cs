using System.Collections.Generic;
using Iesi.Collections;

namespace NHibernate.Validator.Engine
{
	internal interface IClassValidatorImplementor
	{
		InvalidValue[] GetInvalidValues(object bean, ISet circularityState);
		IDictionary<System.Type, IClassValidator> ChildClassValidators { get;}
	}
}