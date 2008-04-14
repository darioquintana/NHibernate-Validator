using System.Windows.Forms;
using NHibernate.Validator.Binding.Util;

namespace NHibernate.Validator.Binding
{
	/// <summary>
	/// 
	/// </summary>
	public class SmartViewValidator : ViewValidator
	{
		private BindControl currentBindingChain;

		public SmartViewValidator(ErrorProvider errorProvider)
			: base(errorProvider)
		{
		}

		public SmartViewValidator() : base()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="control"></param>
		/// <param name="clazz"></param>
		public void Bind(Control control, System.Type clazz)
		{
			Bind(control, clazz, TypeUtil.GetPropertyName(control.Name));
			BindTheEventValidation(control);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="control"></param>
		private void BindTheEventValidation(Control control)
		{
			control.Validating += new EventValidation(this).ValidatingHandler;
		}

		//used for With binding

		public BindControl Bind(System.Type type)
		{
			currentBindingChain = new BindControl(this, type);
			return currentBindingChain;
		}

		#region Nested type: BindControl

		public class BindControl : IHideObjectMembers
		{
			private System.Type clazz;
			private SmartViewValidator smart;

			internal BindControl(SmartViewValidator smart, System.Type clazz)
			{
				this.clazz = clazz;
				this.smart = smart;
			}

			public BindControl With(Control control)
			{
				smart.Bind(control, clazz, TypeUtil.GetPropertyName(control.Name));
				smart.BindTheEventValidation(control);

				return this;
			}
		}

		#endregion
	}
}