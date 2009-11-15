using System;
using System.Collections.Generic;
using NHibernate.Mapping;

namespace NHibernate.Validator.Engine
{
	public interface IClassValidator
	{
		/// <summary>
		/// Return true if this <see cref="ClassValidator"/> contains rules for apply, false in other case. 
		/// </summary>
		bool HasValidationRules { get; }

		/// <summary>
		/// Apply constraints on a entity instance and return all the failures.
		/// if <paramref name="entity"/> is null, an empty array is returned 
		/// </summary>
		/// <param name="entity">object to apply the constraints</param>
		/// <returns></returns>
		IEnumerable<InvalidValue> GetInvalidValues(object entity);

		/// <summary>
		/// Apply constraints on a entity instance and return all the failures for the given property.
		/// if <paramref name="entity"/> is null, an empty array is returned. 
		/// </summary>
		/// <param name="entity">Object to apply the constraints</param>
		/// <param name="propertyName">The name of the property to validate.</param>
		/// <returns></returns>
		IEnumerable<InvalidValue> GetInvalidValues(object entity, string propertyName);

		/// <summary>
		/// Assert a valid Object. A <see cref="NHibernate.Validator.Exceptions.InvalidStateException"/> 
		/// will be throw in a Invalid state.
		/// </summary>
		/// <param name="entity">Object to be asserted</param>
		void AssertValid(object entity);

		/// <summary>
		/// Apply constraints of a particular property value of a entity type and return all the failures.
		/// The InvalidValue objects returns return null for <see cref="InvalidValue.Entity"/> and <see cref="InvalidValue.RootEntity"/>.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IEnumerable<InvalidValue> GetPotentialInvalidValues(string propertyName, object value);

		/// <summary>
		/// Apply the registred constraints rules on the hibernate metadata (to be applied on DB schema)
		/// </summary>
		/// <param name="persistentClass">hibernate metadata</param>
		void Apply(PersistentClass persistentClass);

		/// <summary>
		/// Get the list of constraints declared for a give member of the entityValidator
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns>The list of attribute of the given property.</returns>
		IEnumerable<Attribute> GetMemberConstraints(string propertyName);

		/// <summary>
		/// Apply constraints on a entity instance and return all the failures.
		/// if <paramref name="entity"/> is null, an empty array is returned 
		/// </summary>
		/// <param name="entity">object to apply the constraints</param>
		///<param name="tags">list of tags enabled fpr the validation.</param>
		///<returns>Invalid values</returns>
		IEnumerable<InvalidValue> GetInvalidValues(object entity, params object[] tags);

		/// <summary>
		/// Apply constraints on a entity instance and return all the failures for the given property.
		/// if <paramref name="entity"/> is null, an empty array is returned. 
		/// </summary>
		/// <param name="entity">Object to apply the constraints</param>
		/// <param name="propertyName">The name of the property to validate.</param>
		///<param name="tags">list of tags enabled fpr the validation.</param>
		///<returns>Invalid values</returns>
		IEnumerable<InvalidValue> GetInvalidValues(object entity, string propertyName, params object[] tags);

		/// <summary>
		/// Apply constraints of a particular property value of a entity type and return all the failures.
		/// The InvalidValue objects returns return null for <see cref="InvalidValue.Entity"/> and <see cref="InvalidValue.RootEntity"/>.
		/// </summary>
		/// <param name="propertyName">The name of the property to validate.</param>
		/// <param name="value">the potential value of the property.</param>
		///<param name="tags">list of tags enabled fpr the validation.</param>
		///<returns>Invalid values</returns>
		IEnumerable<InvalidValue> GetPotentialInvalidValues(string propertyName, object value, params object[] tags);
	}
}