using System;
using System.Windows.Forms;
using Iesi.Collections.Generic;
using NHibernate.Validator.Binding.Util;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Binding
{
	public class ViewValidator
	{
		protected ISet<BinderItem> binders = new HashedSet<BinderItem>();
		private ErrorProvider errorProvider;
		private ValidatorControlResolver resolver = new ValidatorControlResolver();
		private ValidatorEngine validatorEngine;

		public ViewValidator()
		{
			if (validatorEngine == null) validatorEngine = new ValidatorEngine();
		}

		public ViewValidator(ErrorProvider errorProvider) : this()
		{
			Check.NotNull(
				errorProvider,
				"errorProvider",
				"The ErrorProvider is null, make sure of construct the ViewValidator after the winforms method InitializeComponent();");
			ErrorProvider = errorProvider;
		}

		public ViewValidator(ValidatorEngine validatorEngine, ErrorProvider errorProvider)
			: this(errorProvider)
		{
			Check.NotNull(
				validatorEngine,
				"ve",
				"The ValidatorEngine is null");
			this.validatorEngine = validatorEngine;
		}

		public ValidatorEngine ValidatorEngine
		{
			get { return validatorEngine; }
		}

		public ErrorProvider ErrorProvider
		{
			get { return errorProvider; }
			set { errorProvider = value; }
		}

		public ValidatorControlResolver Resolver
		{
			get { return resolver; }
		}

		public void Bind(Control control, System.Type entity, string propertyName)
		{
			binders.Add(new BinderItem(control, entity, propertyName));
		}

		public System.Type GetEntityType(Control control)
		{
			foreach (BinderItem item in binders)
			{
				if (control.Equals(item.Control))
					return item.Clazz;
			}
			throw new InvalidOperationException("Could not find the Entity Type for this control");
		}

		public string GetPropertyName(Control control)
		{
			foreach (BinderItem item in binders)
			{
				if (control.Equals(item.Control))
					return item.PropertyName;
			}
			throw new InvalidOperationException("Could not find the Entity Type for this control");
		}
	}
}