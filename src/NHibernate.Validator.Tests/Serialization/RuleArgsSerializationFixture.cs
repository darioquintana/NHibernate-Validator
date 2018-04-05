using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Validator.Constraints;
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
			var implementors = GetValidatorImplementors();
			implementors.ForEach(x => Assert.That(x, Has.Attribute<SerializableAttribute>()));
		}

		[Test]
		public void AllRuleArgsCanBeSerialized()
		{
			TestsContext.AssumeSystemTypeIsSerializable();
			// Test that can be serialized after creation and test default constructor
			var implementors = GetValidatorImplementors();
			foreach (var implementor in implementors)
			{
				object validatorInstance = Activator.CreateInstance(implementor);
				NHAssert.IsSerializable(validatorInstance);
			}
		}

		private static List<System.Type> GetValidatorImplementors()
		{
			Assembly assembly = typeof(IRuleArgs).Assembly;
			var result = new List<System.Type>();
			if (assembly != null)
			{
				System.Type[] types = assembly.GetTypes();
				foreach (System.Type tp in types)
				{
					if (typeof(IValidator).IsAssignableFrom(tp) && !tp.IsInterface && tp.GetConstructor(new System.Type[0]) != null)
						result.Add(tp);
				}
			}
			return result;
		}

		public class Dummy
		{
			public int Value { get; set; }
		}

		[Test]
		public void DelegatedEntityValidatorAttributeIsSerializable()
		{
			TestsContext.AssumeSystemTypeIsSerializable();
			var attributeInstance = new DelegatedValidatorAttribute(new DelegatedConstraint<Dummy>((i, c) => i.Value > 0));
			NHAssert.IsSerializable(attributeInstance);
		}
	}
}
