using System;
using System.Windows.Forms;
using NHibernate.Validator.Demo.Winforms.Model;

namespace NHibernate.Validator.Demo.Winforms
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			IClassValidator vtor = new ClassValidator(typeof (Customer));

			Customer customer = GetFromUI();

			InvalidValue[] values = vtor.GetInvalidValues(customer);
		}

		private Customer GetFromUI()
		{
			throw new NotImplementedException();
		}
	}
}