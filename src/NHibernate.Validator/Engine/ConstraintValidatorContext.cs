using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Validator.Util;

namespace NHibernate.Validator.Engine
{
	public class ConstraintValidatorContext : IConstraintValidatorContext
	{
		private readonly IList<InvalidMessage> invalidValues = new List<InvalidMessage>();
		private readonly string messageAttribute;
		private readonly string propertyName;
		private bool isDisabledDefaultError;

		public ConstraintValidatorContext(string propertyName, string messageAttribute)
		{
			if (messageAttribute == null) throw new ArgumentNullException("messageAttribute");

			this.propertyName = propertyName; //can be null when using Entity-Validation
			this.messageAttribute = messageAttribute;
		}

		public bool IsDefaultValidatorEnabled
		{
			get { return isDisabledDefaultError; }
		}

		public IEnumerable<InvalidMessage> InvalidMessages
		{
			get
			{
				var returnedInvalidMessages = new List<InvalidMessage>(invalidValues);

				if (!isDisabledDefaultError)
					returnedInvalidMessages.Add(new InvalidMessage(DefaultErrorMessage, propertyName));

				return new ReadOnlyCollection<InvalidMessage>(returnedInvalidMessages);
			}
		}

		#region IConstraintValidatorContext Members

		public void DisableDefaultError()
		{
			isDisabledDefaultError = true;
		}

		public string DefaultErrorMessage
		{
			get { return messageAttribute; }
		}

		public void AddInvalid(string message)
		{
			AddInvalid(message, propertyName);
		}

		public void AddInvalid(string message, string property)
		{
			invalidValues.Add(new InvalidMessage(message, property));
		}

		public void AddInvalid<TEntity, TProperty>(string message, Expression<Func<TEntity, TProperty>> property)
		{
			MemberInfo member = TypeUtils.DecodeMemberAccessExpression(property);
			AddInvalid(message, member.Name);
		}

		#endregion
	}

	public class InvalidMessage
	{
		public InvalidMessage(string message, string propertyName)
		{
			Message = message;
			PropertyName = propertyName;
		}

		public string Message { get; private set; }
		public string PropertyName { get; private set; }
	}
}