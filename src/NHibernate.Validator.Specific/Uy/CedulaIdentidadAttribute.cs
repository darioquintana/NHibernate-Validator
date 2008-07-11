using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Specific.Uy
{
	/// <summary>
	/// This expression matches an Uruguayan identity card.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(CedulaIdentidadValidator))]
	public class CedulaIdentidadAttribute : Attribute, IRuleArgs
	{
		private string message = "Cedula de identidad incorrecta.";

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}