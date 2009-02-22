using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Integration
{
	/// <summary>
	/// Interpolator with the prefix '_prefix'
	/// </summary>
	[Serializable]
	public class PrefixMessageInterpolator : IMessageInterpolator
	{
		public string Interpolate(string message, object bean, IValidator validator, IMessageInterpolator defaultInterpolator)
		{
			return "prefix_" + defaultInterpolator.Interpolate(message, bean, validator, defaultInterpolator);
		}
	}
}