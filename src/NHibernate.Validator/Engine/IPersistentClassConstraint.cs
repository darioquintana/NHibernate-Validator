using NHibernate.Mapping;

namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Interface implemented by the validator
	/// when a constraint may be represented in the
	/// NHibernate metadata
	/// </summary>
	public interface IPersistentClassConstraint
	{
		/// <summary>
		///  Apply the constraint in the NHibernate metadata
		/// </summary>
		/// <param name="persistentClass">PersistentClass</param>
		void Apply(PersistentClass persistentClass);
	}
}