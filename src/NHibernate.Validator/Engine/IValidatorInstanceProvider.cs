namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Define the contract for an object that can provide a Validator instance 
	/// instead a <see cref="IValidator"/> <see cref="System.Type"/>.
	/// </summary>
	public interface IValidatorInstanceProvider
	{
		IValidator Validator { get; }
	}
}