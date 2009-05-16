using System;
using System.Windows.Forms;
using NHibernate.Validator.Binding;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Demo.Winforms.Model;
using NHibernate.Validator.Engine;
using Environment=NHibernate.Validator.Cfg.Environment;

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
			//NOTE...IMPORTANT!:
			//This is an example of how-to-change-the-configuration.
			//You should maintain in your code just 1 (one) ValidatorEngine, 
			//see SharedEngine: http://nhforge.org/blogs/nhibernate/archive/2009/02/26/diving-in-nhibernate-validator.aspx
			
			ValidatorEngine ve = vvtor.ValidatorEngine;
			ve.Clear();

			//Configuration of NHV. You can also configure this stuff using xml, outside of the code
			XmlConfiguration nhvc = new XmlConfiguration();
			nhvc.Properties[Environment.ApplyToDDL] = "false";
			nhvc.Properties[Environment.AutoregisterListeners] = "false";
			nhvc.Properties[Environment.ValidatorMode] = GetMode();
			nhvc.Mappings.Add(new MappingConfiguration("NHibernate.Validator.Demo.Winforms",null));
			ve.Configure(nhvc);

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

		private string GetMode()
		{
			if (UseAttributes.Checked)
				return "UseAttribute";

			if (UseXml.Checked)
				return "UseExternal";

			throw new InvalidOperationException();
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
			//Application code here
		}

		private Customer GetCustomerFromUI()
		{
			Customer customer = new Customer();
			customer.FirstName = txtFirstName.Text.Trim();
			customer.LastName = txtLastName.Text.Trim();
			customer.Zip = txtZip.Text.Trim();
			customer.Born = dtpBorn.Value;
			customer.Phone = txtPhone.Text.Trim();
			customer.Email = txtEmail.Text.Trim();
			return customer;
		}
	}
}