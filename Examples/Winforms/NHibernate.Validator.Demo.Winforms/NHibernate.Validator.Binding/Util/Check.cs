using System;

namespace NHibernate.Validator.Binding.Util
{
	public static class Check
	{
		public static void NotNull(object @object) {
			NotNull(@object,null,null);
		}

		public static void NotNull(object @object, string paramName, string message) {
			if (@object == null)
			{
				if (paramName == null || message == null)
					throw new ArgumentNullException();
				else
					throw new ArgumentNullException(paramName, message);
			}
		}
	}
}