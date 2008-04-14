using System.Windows.Forms;
using NHibernate.Validator.Binding.Controls;

namespace NHibernate.Validator.Binding.Controls
{
	[ControlValidable(typeof(DateTimePicker))]
	public class DateTimePickerValuable : IControlValuable
	{
		#region IControlValuable Members

		public object GetValue(Control control)
		{
			return ((DateTimePicker) control).Value;
		}

		#endregion
	}
}