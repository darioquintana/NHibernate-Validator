using System;
using System.Collections.Generic;
using System.Linq;
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
		private readonly ValidatorDef validatorDef;
		private readonly object value;
		private readonly ICollection<object> activeTags;

		public InvalidMessageTransformer(ConstraintValidatorContext constraintContext, ICollection<object> activeTags, System.Type @class, string propertyName,
		                                 object value, object entity, ValidatorDef validatorDef,
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
			if (validatorDef == null)
			{
				throw new ArgumentNullException("validatorDef");
			}
			if (defaultInterpolator == null)
			{
				throw new ArgumentNullException("defaultInterpolator");
			}

			this.constraintContext = constraintContext;
			this.activeTags = activeTags;
			this.@class = @class;
			this.propertyName = propertyName;
			this.value = value;
			this.entity = entity;
			this.validatorDef = validatorDef;
			this.defaultInterpolator = defaultInterpolator;
			this.userInterpolator = userInterpolator;
		}

		public IEnumerable<InvalidValue> Transform()
		{
			return from invalidMsg in constraintContext.InvalidMessages
			       let property = invalidMsg.PropertyName ?? propertyName
			       let interpolatedMessage = Interpolate(property, invalidMsg.Message)
						 select new InvalidValue(interpolatedMessage, @class, property, value, entity, GetMatchTags());
		}

		private string Interpolate(string propName, string message)
		{
			return userInterpolator != null
			       	? userInterpolator.Interpolate(new InterpolationInfo(@class, entity, propName, validatorDef.Validator, defaultInterpolator,
			       	                                                     message))
			       	: defaultInterpolator.Interpolate(new InterpolationInfo(@class, entity, propName, validatorDef.Validator, null, message));
		}

		private ICollection<object> GetMatchTags()
		{
			if (activeTags == null)
			{
				return GetEfectiveValidatorMatchTags();
			}
			return activeTags.Intersect(GetEfectiveValidatorMatchTags()).ToArray();
		}

		private ICollection<object> GetEfectiveValidatorMatchTags()
		{
			return validatorDef.Tags == null || validatorDef.Tags.Count == 0 ? new List<object>() : validatorDef.Tags;
		}
	}
}