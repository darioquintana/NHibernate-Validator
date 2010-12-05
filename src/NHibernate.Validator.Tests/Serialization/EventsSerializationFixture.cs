using System;
using NHibernate.Validator.Event;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Serialization
{
	public class EventsSerializationFixture
	{
		[Test]
		public void AllEventsAreSerializable()
		{
			typeof(ValidatePreInsertEventListener).Should().Have.Attribute<SerializableAttribute>();
			typeof(ValidatePreUpdateEventListener).Should().Have.Attribute<SerializableAttribute>();
		}

		[Test]
		public void AllEventsCanBeSerialized()
		{
			(new ValidatePreInsertEventListener()).Should().Be.BinarySerializable();
			(new ValidatePreUpdateEventListener()).Should().Be.BinarySerializable();
		}
	}
}