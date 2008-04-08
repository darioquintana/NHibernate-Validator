namespace NHibernate.Validator
{
	using System;

    /// <summary>
	/// A single violation of a class level or method level constraint.
	/// </summary>
    [Serializable]
    public class InvalidValue
    {
        private readonly string message;
        private readonly object value;
        private readonly Type beanClass;
        private readonly string propertyName;
        private readonly object bean;
        private object rootBean;
        private string propertyPath;

        public InvalidValue(string message, Type beanClass, string propertyName, object value, object bean)
        {
            this.message = message;
            this.value = value;
            this.beanClass = beanClass;
            this.propertyName = propertyName;
            this.bean = bean;
            rootBean = bean;
            propertyPath = propertyName;
        }

        public void AddParentBean(object parentBean, string propertyName) 
        {
            rootBean = parentBean;
            propertyPath = propertyName + "." + propertyPath;
        }

        public object RootBean
        {
            get { return rootBean; }
        }

        public string PropertyPath
        {
            get { return propertyPath; }
        }

        public Type BeanClass
        {
            get { return beanClass; }
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

        public object Bean
        {
            get { return bean; }
        }

        public override string ToString()
        {
            return string.Concat(propertyName, " ", message);
        }
    }
}