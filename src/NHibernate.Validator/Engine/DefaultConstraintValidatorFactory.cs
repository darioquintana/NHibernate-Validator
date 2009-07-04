using System;

namespace NHibernate.Validator.Engine
{
	[Serializable]
	public class DefaultConstraintValidatorFactory : IConstraintValidatorFactory
	{
		public virtual IValidator GetInstance(System.Type type)
		{
			return (IValidator) Activator.CreateInstance(type);
		}
	}
}