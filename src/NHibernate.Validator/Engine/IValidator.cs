using System;

namespace NHibernate.Validator.Engine
{
	public interface IValidator
	{
		/// <summary>
		/// does the object/element pass the constraints
		/// </summary>
		/// <param name="value">Object to be validated</param>
		/// <returns>if the instance is valid</returns>
		bool IsValid(object value);

		/// <summary>
		/// Take the annotations values and Initialize the Validator
		/// </summary>
		/// <param name="parameters">parameters</param>
		void Initialize(Attribute parameters);
	}
}