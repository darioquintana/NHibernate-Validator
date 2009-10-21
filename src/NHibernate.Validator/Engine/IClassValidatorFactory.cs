using System.Globalization;
using System.Resources;

namespace NHibernate.Validator.Engine
{
	public interface IClassValidatorFactory
	{
		ResourceManager ResourceManager { get; }
		CultureInfo Culture { get; }
		IMessageInterpolator UserInterpolator { get; }
		ValidatorMode ValidatorMode { get; }

		IClassValidator GetRootValidator(System.Type type);
		void GetChildValidator(IClassValidatorImplementor parentValidator, System.Type childType);

		IClassMappingFactory ClassMappingFactory { get; }

		IEntityTypeInspector EntityTypeInspector { get; }
	}
}