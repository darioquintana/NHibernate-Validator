using System;

namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// A single violation of a class level or method level constraint.
	/// </summary>
	/// <remarks>
	/// Created by <see cref="ClassValidator"/>. The ctor is public only for test scope.
	/// </remarks>
	[Serializable]
	public class InvalidValue
	{
		private readonly string message;
		private readonly object value;
		private readonly System.Type entityType;
		private readonly string propertyName;
		private readonly object entity;
		private object rootEntity;
		private string propertyPath;

		public InvalidValue(string message, System.Type entityType, string propertyName, object value, object entity)
		{
			this.message = message;
			this.value = value;
			this.entityType = entityType;
			this.propertyName = propertyName;
			this.entity = entity;
			rootEntity = entity;
			propertyPath = propertyName;
		}

		public void AddParentEntity(object parentEntity, string propertyName) 
		{
			rootEntity = parentEntity;
			propertyPath = propertyName + "." + propertyPath;
		}

		public object RootEntity
		{
			get { return rootEntity; }
		}

		public string PropertyPath
		{
			get { return propertyPath; }
		}

		public System.Type EntityType
		{
			get { return entityType; }
		}

		public string Message
		{
			get { return message; }
		}

		public string PropertyName
		{
			get { return propertyName; }
		}

		public object Value
		{
			get { return value; }
		}

		public object Entity
		{
			get { return entity; }
		}

		public override string ToString()
		{
			return string.Concat(propertyName, "[", message, "]");
		}
	}
}