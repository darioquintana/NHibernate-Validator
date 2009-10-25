namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Contract for Shared Engine Provider
	/// </summary>
	/// <remarks>
	/// The SharedEngineProvider is the service locator for the ValidatorEngine.
	/// More information about why use the Shared Engine Provider are availables in this post :
	/// http://fabiomaulo.blogspot.com/2009/02/diving-in-nhibernatevalidator.html
	/// </remarks>
	public interface ISharedEngineProvider
	{
		/// <summary>
		/// Provide the shared engine instance.
		/// </summary>
		/// <returns>The validator engine.</returns>
		ValidatorEngine GetEngine();
	}
}