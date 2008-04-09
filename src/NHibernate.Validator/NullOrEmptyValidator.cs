using System;
using System.Collections;
using log4net;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class NotNullOrEmptyValidator : IValidator
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(NotNullOrEmptyValidator));
		public bool IsValid(object value)
		{
			log.Info(value == null ? "null" : value.ToString());
			if (value == null) return false;

			if (value is ICollection)
				return ((ICollection)value).Count > 0;

			if (value is string)
				return ((string)value).Length > 0;

			throw new ArgumentException("the object to validate must be a string or a collection", "value");
		}
	}
}
