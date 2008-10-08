using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
	[Serializable]
	public class CreditCardNumberValidator : AbstractLuhnValidator, IValidator
	{
		public override int Multiplicator
		{
			get { return 2; }
		}
	}
}