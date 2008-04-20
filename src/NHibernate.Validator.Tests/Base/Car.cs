using System;

namespace NHibernate.Validator.Tests.Base
{
	public class Car
	{
		public long id;

		[NotEmpty]
		public String name;

		[NotEmpty]
		public String[] insurances;
		  
		[Digits(IntegerDigits = 1, FractionalDigits = 2)]
		public decimal length;

		[Digits(IntegerDigits = 2, FractionalDigits = 1)]
		public double gallons;
	}
}