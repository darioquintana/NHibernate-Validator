using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Assertions
{
	public class CanBeSerializedAsserter : AbstractAsserter
	{
		private readonly object obj;

		public CanBeSerializedAsserter(object obj, string message, params object[] args)
			: base(message, args)
		{
			this.obj = obj;
		}

		public override bool Test()
		{
			bool result = true;
			try
			{
				byte[] bytes = Serialize(obj);
				Deserialize(bytes);
			}
			catch (SerializationException e)
			{
				FailureMessage.WriteLine(string.Format("class {0} is not serializable: {1}", obj.GetType().FullName, e.Message));
				result = false;
			}
			return result;
		}

		public static byte[] Serialize(object obj)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(ms, obj);
				return ms.ToArray();
			}
		}

		public static object Deserialize(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				return formatter.Deserialize(ms);
			}
		}
	}
}
