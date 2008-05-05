using System;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Assertions
{
	public class IsSerializableAsserter : AbstractAsserter
	{
		private readonly System.Type clazz;

		public IsSerializableAsserter(System.Type clazz, string message, params object[] args)
			: base(message, args)
		{
			this.clazz = clazz;
		}

		public IsSerializableAsserter(System.Type clazz)
			: base(string.Empty)
		{
			this.clazz = clazz;
		}

		public override bool Test()
		{
			object[] atts = clazz.GetCustomAttributes(typeof(SerializableAttribute), false);
			return (atts.Length > 0);
		}

		public override string Message
		{
			get
			{
				if (clazz.IsInterface)
					FailureMessage.AddLine(string.Format("The class {0} is an interface.", clazz.FullName));
				else
					FailureMessage.AddLine(string.Format("The class {0} is not marked as Serializable.", clazz.FullName));
				return FailureMessage.ToString();
			}
		}
	}
}
