using System;
using System.Collections.Generic;

namespace NHibernate.Validator.Engine
{
	[Serializable]
	public class ValidatorDef
	{
		public ValidatorDef(IValidator validator, ICollection<object> tags)
		{
			Validator = validator;
			Tags = tags;
		}

		public IValidator Validator { get; private set; }
		public ICollection<object> Tags { get; private set; }
	}
}