using System.Windows.Forms;

namespace NHibernate.Validator.Binding.Controls
{
	/// <summary>
	/// Types that helps to resolve: "where is the value on this control?" 
	/// must implement this interface.
	/// </summary>
	public interface IControlValuable
	{
		object GetValue(Control control);
	}
}