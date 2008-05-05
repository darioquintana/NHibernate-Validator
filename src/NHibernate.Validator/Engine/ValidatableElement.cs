using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using NHibernate.Properties;

namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// The element of an validation.
	/// </summary>
	[Serializable]
	public class ValidatableElement
	{
		private readonly System.Type entityType;
		private readonly IGetter getter;
		private readonly HashedSet<ValidatableElement> subElements = new HashedSet<ValidatableElement>();
		private readonly IClassValidator validator;

		/// <summary>
		/// Create a new instance of <see cref="ValidatableElement"/> for a root element.
		/// </summary>
		/// <param name="entityType">The type of the entity.</param>
		/// <param name="validator">The validator.</param>
		/// <remarks>
		/// The root element is the entity it self.
		/// </remarks>
		public ValidatableElement(System.Type entityType, IClassValidator validator)
		{
			if (entityType == null)
				throw new ArgumentNullException("entityType");
			if (validator == null)
				throw new ArgumentNullException("validator");

			this.entityType = entityType;
			this.validator = validator;
		}

		/// <summary>
		/// Create a new instance of <see cref="ValidatableElement"/> for a composite element.
		/// </summary>
		/// <param name="entityType">The type of the composite element.</param>
		/// <param name="validator">The validator of the composite element.</param>
		/// <param name="getter">The getter of the composite element inside the root entity.</param>
		public ValidatableElement(System.Type entityType, IClassValidator validator, IGetter getter) : this(entityType, validator)
		{
			this.getter = getter;
		}

		/// <summary>
		/// The type of the entity
		/// </summary>
		public System.Type EntityType
		{
			get { return entityType; }
		}

		/// <summary>
		/// The validator of the <see cref="EntityType"/>.
		/// </summary>
		public IClassValidator Validator
		{
			get { return validator; }
		}

		/// <summary>
		/// The getter of the composite element inside the root entity.
		/// </summary>
		public IGetter Getter
		{
			get { return getter; }
		}

		/// <summary>
		/// Composite Elements.
		/// </summary>
		public IEnumerable<ValidatableElement> SubElements
		{
			get { return subElements; }
		}

		/// <summary>
		/// Add a <see cref="ValidatableElement"/>.
		/// </summary>
		/// <param name="subValidatableElement">The sub element</param>
		/// <seealso cref="ValidatableElement(System.Type, IClassValidator, IGetter)"/>
		/// <seealso cref="SubElements"/>
		public void AddSubElement(ValidatableElement subValidatableElement)
		{
			if (subValidatableElement.Getter == null)
				throw new ArgumentException("The sub element of ValidatableElement must have a Getter.", "subValidatableElement");
			subElements.Add(subValidatableElement);
		}

		/// <summary>
		/// Has SubElements ?
		/// </summary>
		public bool HasSubElements
		{
			get { return subElements.Count > 0; }
		}

		public override bool Equals(object obj)
		{
			ValidatableElement that = obj as ValidatableElement;
			if(that == null) 
				return false;
			return entityType.Equals(that.EntityType);
		}

		public override int GetHashCode()
		{
			return entityType.GetHashCode();
		}
	}
}