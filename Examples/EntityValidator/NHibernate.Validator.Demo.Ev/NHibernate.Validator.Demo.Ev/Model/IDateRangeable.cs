using System;
using NHibernate.Validator.Demo.Ev.Validators;

namespace NHibernate.Validator.Demo.Ev.Model
{
	[ValidRange]
	public interface IDateRangeable
	{
		DateTime Start { get; set; }
		DateTime End { get; set; }
	}
}