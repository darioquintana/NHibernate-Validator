using System;
using System.Collections.Generic;
using NHibernate.Validator.Interpolator;

namespace NHibernate.Validator.Engine
{
	public class InvalidMessageTransformer
	{
		private readonly object entity;
		private readonly IValidator validator;
		private readonly DefaultMessageInterpolatorAggregator defaultInterpolator;
		private readonly IMessageInterpolator userInterpolator;
		private readonly System.Type @class;
		private readonly ConstraintValidatorContext constraintContext;
		private readonly string propertyName;
		private readonly object value;

		public InvalidMessageTransformer(ConstraintValidatorContext constraintContext, 
			System.Type @class, 
			string propertyName /* nullable */, 
			object value /* nullable */,
			object entity /* nullable */,
			IValidator validator,
			DefaultMessageInterpolatorAggregator defaultInterpolator,
			IMessageInterpolator userInterpolator /* nullable */)
		{
			if (constraintContext == null) throw new ArgumentNullException("constraintContext");
			if (@class == null) throw new ArgumentNullException("class");
			if (validator == null) throw new ArgumentNullException("validator");
			if (defaultInterpolator == null) throw new ArgumentNullException("defaultInterpolator");

			this.constraintContext = constraintContext;
			this.@class = @class;
			this.propertyName = propertyName;
			this.value = value;
			this.entity = entity;
			this.validator = validator;
			this.defaultInterpolator = defaultInterpolator;
			this.userInterpolator = userInterpolator;
		}

		public IEnumerable<InvalidValue> Transform()
		{
			foreach (var invalidMsg in constraintContext.InvalidMessages)
			{
				var interpolatedMessage = Interpolate(entity, invalidMsg.Message, validator);
				var property = invalidMsg.PropertyName ?? propertyName;
				yield return new InvalidValue(interpolatedMessage, @class, property, value, entity);
			}
		}

		private string Interpolate(object entity, string message, IValidator validator)
		{
			if (userInterpolator != null)
			{
				return userInterpolator.Interpolate(message, entity, validator, defaultInterpolator);
			}
			else
			{
				return defaultInterpolator.Interpolate(message, entity, validator, null);
			}
		}
	}
}