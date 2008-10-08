using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Br
{
	/// <summary>
	/// This expression matches the Brazilian Zip-Code (CEP)
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof (CPFValidator))]
	public class CPFAttribute : Attribute, IRuleArgs
	{
		private string message = "Número de CPF inválido.";

		#region IRuleArgs Members

		public string Message
		{
			get { return message; }
			set { message = value; }
		}

		#endregion
	}
}