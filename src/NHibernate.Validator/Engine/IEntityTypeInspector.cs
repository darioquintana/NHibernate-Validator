namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Inspector to find the real type of an entity instance.
	/// </summary>
	/// <remarks>
	/// En entity instace can be a dynamic proxy.
	/// Implementors shoud recognize the proxy and return the <see cref="System.Type"/>
	/// of the target.
	/// If the type is not a proxy the implementors shouldn't recognize it.
	/// </remarks>
	public interface IEntityTypeInspector
	{
		/// <summary>
		/// Guess the type of the entity instance.
		/// </summary>
		/// <param name="entityInstance">The entity instance</param>
		/// <returns>The <see cref="System.Type"/> of the entity if was recognized; otherwise null.</returns>
		System.Type GuessType(object entityInstance);
	}
}