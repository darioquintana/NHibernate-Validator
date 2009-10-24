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
		public string Interpolate(InterpolationInfo info)
		{
			return "prefix_" + info.DefaultInterpolator.Interpolate(info);
		}
	}
}