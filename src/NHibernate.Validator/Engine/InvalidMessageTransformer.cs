using System;
using System.Collections.Generic;
using NHibernate.Validator.Interpolator;

namespace NHibernate.Validator.Engine
{
	public class InvalidMessageTransformer
	{
		private readonly System.Type @class;
		private readonly ConstraintValidatorContext constraintContext;
		private readonly DefaultMessageInterpolatorAggregator defaultInterpolator;
		private readonly object entity;
		private readonly string propertyName;
		private readonly IMessageInterpolator userInterpolator;
		private readonly IValidator validator;
		private readonly object value;

		public InvalidMessageTransformer(ConstraintValidatorContext constraintContext, System.Type @class, string propertyName,
		                                 object value, object entity, IValidator validator,
		                                 DefaultMessageInterpolatorAggregator defaultInterpolator,
		                                 IMessageInterpolator userInterpolator)
		{
			if (constraintContext == null)
			{
				throw new ArgumentNullException("constraintContext");
			}
			if (@class == null)
			{
				throw new ArgumentNullException("class");
			}
			if (validator == null)
			{
				throw new ArgumentNullException("validator");
			}
			if (defaultInterpolator == null)
			{
				throw new ArgumentNullException("defaultInterpolator");
			}

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
			foreach (InvalidMessage invalidMsg in constraintContext.InvalidMessages)
			{
				string property = invalidMsg.PropertyName ?? propertyName;
				string interpolatedMessage = Interpolate(property, invalidMsg.Message);
				yield return new InvalidValue(interpolatedMessage, @class, property, value, entity);
			}
		}

		private string Interpolate(string propName, string message)
		{
			return userInterpolator != null
			       	? userInterpolator.Interpolate(new InterpolationInfo(@class, entity, propName, validator, defaultInterpolator,
			       	                                                     message))
			       	: defaultInterpolator.Interpolate(new InterpolationInfo(@class, entity, propName, validator, null, message));
		}
	}
}