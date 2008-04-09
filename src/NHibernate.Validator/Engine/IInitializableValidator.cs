using System;
namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Contract for validator that need initialization.
	/// </summary>
	/// <typeparam name="A">The attribute that hold validator parameters.</typeparam>
	public interface IInitializableValidator<A>: IValidator where A: Attribute
	{
		/// <summary>
		/// Take the attribute values and Initialize the Validator
		/// </summary>
		/// <param name="parameters">parameters</param>
		void Initialize(A parameters);
	}
}