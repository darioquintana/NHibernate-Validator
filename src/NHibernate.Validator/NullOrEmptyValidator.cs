using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using log4net;

namespace NHibernate.Validator
{
	public class NotNullOrEmptyValidator : Validator<NotNullOrEmptyAttribute>
	{
		private static ILog log = LogManager.GetLogger(typeof(NotNullOrEmptyValidator));
		public override bool IsValid(object value)
		{
			log.Info(value == null ? "null" : value.ToString());
			if (value == null) return false;

			if (value is ICollection)
				return ((ICollection)value).Count > 0;

			if (value is string)
				return ((string)value).Length > 0;

			throw new ArgumentException("the object to validate must be a string or a collection", "value");
		}

		public override void Initialize(NotNullOrEmptyAttribute parameters)
		{
			
		}
	}
}
