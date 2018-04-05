using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;

using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Interpolator
{
	[Serializable]
	public class DefaultMessageInterpolatorAggregator : IMessageInterpolator, ISerializable
	{
		private readonly IDictionary<IValidator, DefaultMessageInterpolator> interpolators;
			
		//transient but repopulated by the object owing a reference to the interpolator
		[NonSerialized] private CultureInfo culture;
		[NonSerialized] private ResourceManager defaultMessageBundle;
		[NonSerialized] private ResourceManager messageBundle;
		[NonSerialized] private bool isInitilized;

		#region IMessageInterpolator Members

		public string Interpolate(InterpolationInfo info)
		{
			CheckInitialized();

			DefaultMessageInterpolator defaultMessageInterpolator;
			if (!interpolators.TryGetValue(info.Validator, out defaultMessageInterpolator))
			{
				return info.Message;
			}

			return defaultMessageInterpolator.Interpolate(info);
		}

		private void CheckInitialized()
		{
			lock (this)
			{
				if (!isInitilized)
				{
					foreach (DefaultMessageInterpolator interpolator in interpolators.Values)
					{
						interpolator.Initialize(messageBundle, defaultMessageBundle, culture);
					}

					isInitilized = true;
				}
			}
		}

		#endregion

		public void Initialize(ResourceManager messageBundle, ResourceManager defaultMessageBundle, CultureInfo culture)
		{
			this.culture = culture;
			this.messageBundle = messageBundle;
			this.defaultMessageBundle = defaultMessageBundle;
		}

		public void AddInterpolator(Attribute attribute, IValidator validator)
		{
			var interpolator = new DefaultMessageInterpolator();
			interpolator.Initialize(messageBundle, defaultMessageBundle, culture);
			interpolator.Initialize(attribute);
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

		public DefaultMessageInterpolatorAggregator()
		{
			interpolators = new Dictionary<IValidator, DefaultMessageInterpolator>(new ValidatorReferenceEqualityComparer());
		}

		public DefaultMessageInterpolatorAggregator(SerializationInfo info, StreamingContext context)
		{
			interpolators = (IDictionary<IValidator, DefaultMessageInterpolator>)
				info.GetValue("interpolators", typeof(IDictionary<IValidator, DefaultMessageInterpolator>));
		}

		[SecurityCritical]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("interpolators", interpolators);
		}

		[Serializable]
		private class ValidatorReferenceEqualityComparer : IEqualityComparer<IValidator>
		{
			public bool Equals(IValidator x, IValidator y)
			{
				return ReferenceEquals(x, y);
			}

			public int GetHashCode(IValidator obj)
			{
				return RuntimeHelpers.GetHashCode(obj);
			}
		}
	}
}