namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Contract for Shared Engine Provider
	/// </summary>
	public interface ISharedEngineProvider
	{
		/// <summary>
		/// Provide the shared engine instance.
		/// </summary>
		/// <returns>The validator engine.</returns>
		ValidatorEngine GetEngine();
	}
}