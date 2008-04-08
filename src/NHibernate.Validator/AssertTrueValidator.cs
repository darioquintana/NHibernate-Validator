using System;

namespace NHibernate.Validator
{
	public class AssertTrueValidator : Validator<AssertTrueAttribute>
	{
		public override bool IsValid(Object value)
		{
			return (bool) value;
		}

		public override void Initialize(AssertTrueAttribute parameters)
		{
		}
	}
}