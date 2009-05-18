using System;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Demo.Ev.Model
{
	public class Meeting : IDateRangeable
	{
		[NotNullNotEmpty]
		public string Name { get; set; }

		[NotNullNotEmpty]
		public string Description { get; set; }

		public DateTime Start { get; set; }
		public DateTime End { get; set; }
	}
}