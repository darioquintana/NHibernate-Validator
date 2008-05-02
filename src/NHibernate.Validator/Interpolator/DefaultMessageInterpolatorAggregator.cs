using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Runtime.Serialization;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Interpolator
{
	[Serializable]
	public class DefaultMessageInterpolatorAggregator : IMessageInterpolator
	{
		private readonly IDictionary<IValidator, DefaultMessageInterpolator> interpolators =
			new Dictionary<IValidator, DefaultMessageInterpolator>();

		//transient but repopulated by the object owing a reference to the interpolator
		[NonSerialized] private CultureInfo culture;
		[NonSerialized] private ResourceManager defaultMessageBundle;
		[NonSerialized] private ResourceManager messageBundle;

		#region IMessageInterpolator Members

		public string Interpolate(string message, IValidator validator, IMessageInterpolator defaultInterpolator)
		{
			DefaultMessageInterpolator defaultMessageInterpolator;

			if (!interpolators.TryGetValue(validator, out defaultMessageInterpolator))
			{
				return message;
			}

			return defaultMessageInterpolator.Interpolate(message, validator, defaultInterpolator);
		}

		#endregion

		public void Initialize(ResourceManager messageBundle, ResourceManager defaultMessageBundle, CultureInfo culture)
		{
			this.culture = culture;
			this.messageBundle = messageBundle;
			this.defaultMessageBundle = defaultMessageBundle;
		}

		[OnDeserialized]
		private void DeserializationCallBack(StreamingContext context)
		{
			foreach (DefaultMessageInterpolator interpolator in interpolators.Values)
			{
				interpolator.Initialize(messageBundle, defaultMessageBundle, culture);
			}
		}

		public void AddInterpolator(Attribute attribute, IValidator validator)
		{
			DefaultMessageInterpolator interpolator = new DefaultMessageInterpolator();
			interpolator.Initialize(messageBundle, defaultMessageBundle, culture);
			interpolator.Initialize(attribute, null);
			interpolators[validator] = interpolator;
		}

		public string GetAttributeMessage(IValidator validator)
		{
			DefaultMessageInterpolator defaultMessageInterpolator;
			if (interpolators.TryGetValue(validator, out defaultMessageInterpolator))
			{
				return defaultMessageInterpolator.AttributeMessage;
			}
			else
			{
				throw new AssertionFailureException("Validator not registred to the MessageInterceptorAggregator");
			}
		}
	}
}