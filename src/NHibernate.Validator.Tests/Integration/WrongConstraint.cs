using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Integration
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(WrongConstraint))]
	public class WrongConstraintAttribute : Attribute, IRuleArgs
	{
		public string Message
		{
			get { return string.Empty; }
			set { }
		}
	}

	public class WrongConstraint : IValidator, IPersistentClassConstraint, IPropertyConstraint
	{
		public bool IsValid(object value)
		{
			//Always no valid, then the message can be used, and then an exception must be thorwn.
			return false;
		}

		#region IPropertyConstraint Members

		public void Apply(Mapping.Property property)
		{
			throw new Exception("Any kind of exception");
		}

		#endregion

		#region IPersistentClassConstraint Members

		public void Apply(Mapping.PersistentClass persistentClass)
		{
			throw new Exception("Any kind of exception");
		}

		#endregion
	}
}