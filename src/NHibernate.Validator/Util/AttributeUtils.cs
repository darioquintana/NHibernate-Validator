using System;

namespace NHibernate.Validator.Util
{
	/// <summary>
	/// Utils metods for attributes
	/// </summary>
	public static class AttributeUtils
	{
		/// <summary>
		/// Returns true if the attribute can be declared more than one time for the same element
		/// </summary>
		/// <param name="attribute"></param>
		/// <returns></returns>
		public static bool AttributeAllowsMultiple(Attribute attribute)
		{
			Attribute usageAttribute = Attribute.GetCustomAttribute(attribute.GetType(), typeof(AttributeUsageAttribute));
			return ((AttributeUsageAttribute)usageAttribute).AllowMultiple;
		}
	}
}
