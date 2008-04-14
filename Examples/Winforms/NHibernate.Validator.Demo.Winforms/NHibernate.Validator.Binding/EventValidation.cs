using System.ComponentModel;
using System.Windows.Forms;
using NHibernate.Validator.Binding.Controls;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Binding
{
	public class EventValidation
	{
		private ErrorProvider errorProvider;
		private ViewValidator vvtor;
		private ValidatorEngine validatorEngine;

		public EventValidation(ViewValidator viewValidator)
		{
			this.vvtor = viewValidator;
			this.errorProvider = viewValidator.ErrorProvider;
			this.validatorEngine = viewValidator.ValidatorEngine;
		}

		public void ValidatingHandler(object sender, CancelEventArgs e)
		{
			//TODO: reimplement this:
			//System.Type entityType = vvtor.GetEntityType((Control) sender);
			
			//IControlValuable controlValuable = vvtor.Resolver.GetControlValuable(sender);

			//InvalidValue[] errors =
			//    validatorEngine.ValidatePropertyValue(GetPropertyName((Control) sender), controlValuable.GetValue((Control) sender));

			//if (errors.Length > 0)
			//    errorProvider.SetError((TextBox) sender, errors[0].Message);
			//else
			//    errorProvider.SetError((TextBox) sender, string.Empty);
		}

		private string GetPropertyName(Control control)
		{
			return vvtor.GetPropertyName(control);
		}
	}
}