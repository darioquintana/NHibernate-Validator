namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Contract for Shared EngineP rovider
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