using NHibernate.Mapping;

namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Interface implemented by the validator
	/// when a constraint may be represented in a
	/// hibernate metadata property
	/// </summary>
	public interface IPropertyConstraint
	{
		void Apply(Property property);
	}
}