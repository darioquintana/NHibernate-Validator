using System;
using System.Collections.Generic;

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
		public InvalidValue(string message, System.Type entityType, string propertyName, object value, object entity, ICollection<object> matchTags)
		{
			Message = message;
			Value = value;
			EntityType = entityType;
			PropertyName = propertyName;
			Entity = entity;
			MatchTags = matchTags;
			RootEntity = entity;
			PropertyPath = propertyName;
		}

		public object RootEntity { get; private set; }

		public string PropertyPath { get; private set; }

		public System.Type EntityType { get; private set; }

		public string Message { get; private set; }

		public string PropertyName { get; private set; }

		public object Value { get; private set; }

		public object Entity { get; private set; }

		public ICollection<object> MatchTags { get; private set; }

		public void AddParentEntity(object parentEntity, string propertyName)
		{
			RootEntity = parentEntity;

			if (PropertyPath == null) //NHV-93
				PropertyPath = propertyName;
			else
			{
				PropertyPath = propertyName + "." + PropertyPath;
			}
		}

		public override string ToString()
		{
			return string.Concat(PropertyName, "[", Message, "]");
		}
	}
}