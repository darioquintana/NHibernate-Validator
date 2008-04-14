using System;
using System.Windows.Forms;
using NHibernate.Validator.Binding;
using NHibernate.Validator.Demo.Winforms.Model;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Demo.Winforms
{
	public partial class Form1 : Form
	{
		SmartViewValidator vvtor;
		ValidatorEngine ve = new ValidatorEngine();

		public Form1()
		{
			InitializeComponent();

			//Binding with NHibernate.Validator process.

			//Setting the ErrorProvider to the binder.
			vvtor = new SmartViewValidator(errorProvider1);

			//Telling who are the Controls and who is the entity Type to bind
			vvtor.Bind(typeof (Customer))
				.With(txtFirstName)
				.With(txtLastName)
				.With(txtEmail)
				.With(txtPhone)
				.With(txtZip);
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			Customer customer = GetFromUI();
			

			if (ve.IsValid(customer))
			{
				listBox1.Items.Clear();
				Send();
			}
			else
			{
				InvalidValue[] values = ve.Validate(customer);
				FillErrorsOnListBox(values);
			}
		}

		private void FillErrorsOnListBox(InvalidValue[] values)
		{
			listBox1.Items.Clear();

			foreach (InvalidValue value in values)
			{
				string message = value.PropertyName + ": " + value.Message;

				listBox1.Items.Add(message);
			}
		}

		public void Send()
		{
			//Code here
		}

		private Customer GetFromUI()
		{
			Customer customer = new Customer();
			customer.FirstName = txtFirstName.Text.Trim();
			customer.LastName = txtLastName.Text.Trim();
			customer.Zip = txtZip.Text.Trim();
			customer.Born = dtpBorn.Value;
			return customer;
		}
	}
}