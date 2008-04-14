using System;

namespace NHibernate.Validator.Binding.Util
{
	public class TypeUtil
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string GetPropertyName(string name)
		{
			if (!char.IsLower(name[0]))
			{
				throw new InvalidOperationException(
					"The first letter of the name must be lower case in order to extract the Property Name");
			}

			for(int i = 0; i < name.Length; i++)
			{
				if (char.IsUpper(name[i])) return name.Substring(i);
			}

			throw new ArgumentException("Could not extract the property from the parameter","name");
		}
	}
}