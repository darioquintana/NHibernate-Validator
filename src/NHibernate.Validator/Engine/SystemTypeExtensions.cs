namespace NHibernate.Validator.Engine
{
	internal static class SystemTypeExtensions
	{
		internal static bool ShouldNeedValidation(this System.Type clazz)
		{
			return (!clazz.FullName.StartsWith("System") && !clazz.IsValueType);
		}
	}
}