namespace NHibernate.Validator.Engine
{
	public class InterpolationInfo
	{
		public InterpolationInfo(System.Type entity, object entityInstance, string propertyName, IValidator validator,
		                         IMessageInterpolator defaultInterpolator, string message)
		{
			EntityInstance = entityInstance;
			Validator = validator;
			DefaultInterpolator = defaultInterpolator;
			Entity = entity;
			PropertyName = propertyName;
			Message = message;
		}

		public System.Type Entity { get; private set; }

		public string PropertyName { get; private set; }

		public object EntityInstance { get; private set; }

		public IValidator Validator { get; private set; }

		public IMessageInterpolator DefaultInterpolator { get; private set; }

		public string Message { get; set; }
	}
}