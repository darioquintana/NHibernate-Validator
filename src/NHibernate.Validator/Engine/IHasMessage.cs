namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// All validators attributes must implement this interface.
	/// </summary>
	public interface IHasMessage
	{
		string Message { get; set; }
	}
}