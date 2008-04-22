namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// All validators attributes must implement this interface.
	/// </summary>
	public interface IRuleArgs
	{
		string Message { get; set; }
	}
}