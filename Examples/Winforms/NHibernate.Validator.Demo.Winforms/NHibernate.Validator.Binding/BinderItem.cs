using System.Windows.Forms;

namespace NHibernate.Validator.Binding
{
	public class BinderItem
	{
		private System.Type clazz;
		private Control control;
		private string propertyName;

		public BinderItem(Control control, System.Type clazz, string propertyName)
		{
			this.control = control;
			this.clazz = clazz;
			this.propertyName = propertyName;
		}

		public Control Control
		{
			get { return control; }
			set { control = value; }
		}

		public System.Type Clazz
		{
			get { return clazz; }
			set { clazz = value; }
		}

		public string PropertyName
		{
			get { return propertyName; }
			set { propertyName = value; }
		}
	}
}