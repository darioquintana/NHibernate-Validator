using System;
using NHibernate.Validator.Demo.Ev.Model;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Demo.Ev.Validators
{
	public class ValidRangeValidator : IValidator
	{
		public bool IsValid(object value)
		{
			var entity = (IDateRangeable) value;

			int result = DateTime.Compare(entity.Start, entity.End);

			if (result == 0 || result > 0)
				return false;

			return true;
		}
	}
}