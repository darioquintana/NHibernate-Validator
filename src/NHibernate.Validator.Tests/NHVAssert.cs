using NHibernate.Validator.Tests.Assertions;
using NUnit.Framework;

namespace NHibernate.Validator.Tests
{
	public class NHVAssert
	{
		#region Serializable

		public static void IsSerializable(System.Type clazz)
		{
			IsSerializable(clazz, null, null);
		}

		public static void IsSerializable(System.Type clazz, string message, params object[] args)
		{
			Assert.DoAssert(new IsSerializableAsserter(clazz, message, args));
		}

		public static void InheritedAreSerializable(System.Type clazz)
		{
			InheritedAreSerializable(clazz, null, null);
		}

		public static void InheritedAreSerializable(System.Type clazz, string message, params object[] args)
		{
			Assert.DoAssert(new InheritedAreSerializableAsserter(clazz, message, args));
		}

		public static void CanBeSerialized(object obj)
		{
			CanBeSerialized(obj, null, null);
		}

		public static void CanBeSerialized(object obj, string message, params object[] args)
		{
			Assert.DoAssert(new CanBeSerializedAsserter(obj, message, args));
		}

		#endregion
	}
}
