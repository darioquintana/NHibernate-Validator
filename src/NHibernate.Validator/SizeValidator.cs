using System.Collections;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	public class SizeValidator : AbstractValidator<SizeAttribute>
	{
		private int min;
		private int max;


		public override bool IsValid(object value)
		{
			ICollection collection = value as ICollection;

			if(collection == null) return true;

			return collection.Count >= min && collection.Count <= max;
		}

		public override void Initialize(SizeAttribute parameters)
		{
			min = parameters.Min;
			max = parameters.Max;
		}
	}
}