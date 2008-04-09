using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Integration
{
	using System;

	/// <summary>
	/// Interpolator with the prefix '_prefix'
	/// </summary>
	public class PrefixMessageInterpolator : IMessageInterpolator
	{
		public string Interpolate(string message, IValidator validator, IMessageInterpolator defaultInterpolator)
		{
			return "prefix_" + defaultInterpolator.Interpolate(message, validator, defaultInterpolator);
		}
	}
}