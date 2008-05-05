using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Serialization
{
	[TestFixture]
	public class RuleArgsSerializationFixture
	{
		[Test]
		public void AllRuleArgsAreSerializable()
		{
			NHVAssert.InheritedAreSerializable(typeof(IRuleArgs));
		}

		[Test]
		public void AllRuleArgsCanBeSerialized()
		{
			// Test that can be serialized after creation and test default constructor
			foreach (System.Type implementor in GetValidatorImplementors())
			{
				object validatorInstance = Activator.CreateInstance(implementor);
				NHVAssert.CanBeSerialized(validatorInstance);
			}
		}

		private static IList<System.Type> GetValidatorImplementors()
		{
			Assembly assembly = typeof(IValidator).Assembly;
			IList<System.Type> result = new List<System.Type>();
			if (assembly != null)
			{
				System.Type[] types = assembly.GetTypes();
				foreach (System.Type tp in types)
				{
					if (typeof(IRuleArgs).IsAssignableFrom(tp) && !tp.IsInterface)
						result.Add(tp);
				}
			}
			return result;
		}
	}
}
