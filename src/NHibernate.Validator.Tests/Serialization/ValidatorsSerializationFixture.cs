using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Serialization
{
	[TestFixture]
	public class ValidatorsSerializationFixture
	{
		[Test]
		public void AllValidatorsAreSerializable()
		{
			var implementors = GetValidatorImplementors();
			implementors.ForEach(x => Assert.That(x, Has.Attribute<SerializableAttribute>()));
		}

		[Test]
		public void AllValidatorsCanBeSerialized()
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

		[Test, Ignore("Test not Implemented.")]
		public void AllValidatorsCanBeSerializedAfterInitialization()
		{
			// TODO : Test can be serialized after initialization
		}

		private static List<System.Type> GetValidatorImplementors()
		{
			Assembly assembly = typeof (IValidator).Assembly;
			List<System.Type> result = new List<System.Type>();
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
		public void DelegatedConstraintIsSerializable()
		{
			TestsContext.AssumeSystemTypeIsSerializable();
			var validatorInstance = new DelegatedConstraint<Dummy>((i, c) => i.Value > 0);
			NHAssert.IsSerializable(validatorInstance);
		}
	}
}
