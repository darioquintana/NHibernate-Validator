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
		
		public Form1()
		{
			InitializeComponent();

			//Binding with NHibernate.Validator process.

			//Setting the ErrorProvider to the binder.
			vvtor = new SmartViewValidator(errorProvider1);

			//What are the Controls and who are the entity Type to bind to?
			vvtor.Bind(typeof (Customer))
				.With(txtFirstName)
				.With(txtLastName)
				.With(txtEmail)
				.With(txtPhone)
				.With(txtZip);
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			ValidatorEngine ve = vvtor.ValidatorEngine;
			//ve.

			Customer customer = GetCustomerFromUI();

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

		private Customer GetCustomerFromUI()
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