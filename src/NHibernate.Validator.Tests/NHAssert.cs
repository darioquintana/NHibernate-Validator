#if !NETFX
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
#endif
using NUnit.Framework;

namespace NHibernate.Validator.Tests
{
	public static class NHAssert
	{
		public static void IsSerializable(object obj)
		{
			IsSerializable(obj, null, null);
		}

		public static void IsSerializable(object obj, string message, params object[] args)
		{
#if NETFX
			Assert.That(obj, Is.BinarySerializable, message, args);
#else
			if (obj == null) throw new ArgumentNullException(nameof(args));
			var serializer = new BinaryFormatter();
			var isSuccess = false;
			using (var memoryStream = new MemoryStream())
			{
				Assert.That(
					() =>
					{
						serializer.Serialize(memoryStream, obj);
						memoryStream.Seek(0L, SeekOrigin.Begin);
						var deserialized = serializer.Deserialize(memoryStream);
						// ReSharper disable once ConditionIsAlwaysTrueOrFalse
						isSuccess = deserialized != null;
					}, Throws.Nothing);
			}

			Assert.That(isSuccess, message, args);
#endif
		}
	}
}
