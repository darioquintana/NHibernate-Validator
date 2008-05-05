using System;
using System.Collections;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	[Serializable]
	public class SizeValidator : IInitializableValidator<SizeAttribute>
	{
		private int max;
		private int min;

		public bool IsValid(object value)
		{
			if(value == null) return true;

			ICollection collection = value as ICollection;
			if (collection == null)
			{
				return false;
			}

			return collection.Count >= min && collection.Count <= max;
		}

		public void Initialize(SizeAttribute parameters)
		{
			min = parameters.Min;
			max = parameters.Max;
		}
	}
}