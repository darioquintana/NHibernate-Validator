using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator
{
	/// <summary>
	/// Not null constraint
	/// </summary>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    [ValidatorClass(typeof(NotNullValidator))]
	public class NotNullAttribute : Attribute, IHasMessage
    {
        private string message = "{validator.notEmpty}";

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}