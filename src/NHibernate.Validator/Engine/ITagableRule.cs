using System.Collections.Generic;

namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Define a validator-definition (Attribute) which accept tags.
	/// </summary>
	public interface ITagableRule
	{
		ICollection<object> TagCollection { get; }
	}
}