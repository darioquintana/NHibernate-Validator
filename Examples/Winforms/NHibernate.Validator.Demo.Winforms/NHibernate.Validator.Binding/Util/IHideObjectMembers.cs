using System.ComponentModel;

namespace NHibernate.Validator.Binding.Util
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IHideObjectMembers
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		System.Type GetType();

		[EditorBrowsable(EditorBrowsableState.Never)]
		int GetHashCode();

		[EditorBrowsable(EditorBrowsableState.Never)]
		string ToString();

		[EditorBrowsable(EditorBrowsableState.Never)]
		bool Equals(object obj);
	}
}