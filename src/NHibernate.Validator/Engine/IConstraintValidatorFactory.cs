namespace NHibernate.Validator.Engine
{
	public interface IConstraintValidatorFactory
	{
		IValidator GetInstance(System.Type type);
	}
}