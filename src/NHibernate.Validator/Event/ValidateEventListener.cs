using System;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Event
{
	/// <summary>
	/// Common environment for insert and update NH event listeners
	/// </summary>
	[Serializable]
	public class ValidateEventListener
	{
		[NonSerialized]
		private static readonly object padlock = new object();

		private static ValidatorEngine ve; // engine for listeners

		protected static void Validate(object entity, EntityMode mode)
		{
			if (entity == null || mode != EntityMode.Poco)
			{
				return;
			}

			InvalidValue[] consolidatedInvalidValues = Engine.Validate(entity);
			if (consolidatedInvalidValues.Length > 0)
			{
				throw new InvalidStateException(consolidatedInvalidValues, entity.GetType().Name);
			}
		}

		protected static ValidatorEngine Engine
		{
			get
			{
				lock (padlock)
				{
					if (ve == null)
					{
						ve = new ValidatorEngine();
						ve.Configure(); // configure the private ValidatorEngine
					}
				}
				return ve;
			}
			set
			{
				lock (padlock)
				{
					ve = value;
				}
			}
		}
	}
}