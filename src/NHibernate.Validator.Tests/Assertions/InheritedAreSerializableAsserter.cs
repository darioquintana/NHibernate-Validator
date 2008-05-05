using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Assertions
{
	public class InheritedAreSerializableAsserter : AbstractAsserter
	{
		private readonly System.Type type;

		public InheritedAreSerializableAsserter(System.Type type, string message, params object[] args)
			: base(message, args)
		{
			this.type = type;
		}

		public override bool Test()
		{
			int failedCount = 0;
			Assembly nhbA = Assembly.GetAssembly(type);
			IList<System.Type> types = ClassList(nhbA);
			foreach (System.Type tp in types)
			{
				if (!tp.IsSerializable)
				{
					FailureMessage.AddLine(string.Format("    class {0} is not Serializable", tp.FullName));
					failedCount++;
				}
			}
			return (failedCount == 0);
		}

		private IList<System.Type> ClassList(Assembly assembly)
		{
			IList<System.Type> result = new List<System.Type>();
			if (assembly != null)
			{
				System.Type[] types = assembly.GetTypes();
				foreach (System.Type tp in types)
				{
					if (tp != type && type.IsAssignableFrom(tp) && !tp.IsInterface)
						result.Add(tp);
				}
			}
			return result;
		}
	}
}
