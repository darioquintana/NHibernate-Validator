using System;

namespace NHibernate.Validator.Demo.Winforms.Model
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(ZipAttribute))]
	public class ZipAttribute : Attribute, IHasMessage
	{
		private string message = string.Empty;

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}