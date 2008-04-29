using System;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Tests.Configuration
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	[ValidatorClass(typeof(ACustomValidator))]
	internal class ACustomAttribute: Attribute
	{
		private string value1;
		private int value2;
		private string message;

		public string Value1
		{
			get { return value1; }
			set { value1 = value; }
		}

		public int Value2
		{
			get { return value2; }
			set { value2 = value; }
		}

		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}
}
