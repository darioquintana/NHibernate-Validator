using System;
using System.Globalization;
using System.Resources;

namespace NHibernate.Validator.Interpolator
{
	public interface IInitializableMessageInterpolator
	{
		void Initialize(ResourceManager messageBundle, ResourceManager defaultMessageBundle, CultureInfo culture);
		void Initialize(Attribute attribute);
	}
}