using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Event
{
	/// <summary>
	/// Common environment for insert and update NH event listeners
	/// </summary>
	public class ValidateEventListener
	{
		protected static ValidatorEngine ve; // engine for listeners

		protected static void Validate(object entity, EntityMode mode)
		{
			if (entity == null || mode != EntityMode.Poco)
			{
				return;
			}

			InvalidValue[] consolidatedInvalidValues = ve.Validate(entity);
			if (consolidatedInvalidValues.Length > 0)
			{
				throw new InvalidStateException(consolidatedInvalidValues, entity.GetType().Name);
			}
		}
	}
}