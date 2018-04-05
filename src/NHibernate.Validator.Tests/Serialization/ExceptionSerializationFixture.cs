using System;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Serialization
{
	[TestFixture]
	public class ExceptionSerializationFixture
	{
		[Test]
		public void AssertionFailure()
		{
			var ex = new AssertionFailure();
			Assert.That(
				() => SerializationHelper.Deserialize(SerializationHelper.Serialize(ex)),
				Throws.Nothing);
		}

		[Test]
		public void InvalidAttributeNameException()
		{
			var ex = new InvalidAttributeNameException("blah", typeof(ExceptionSerializationFixture));
			var dex = default(InvalidAttributeNameException);
			Assert.That(
				() => { dex = (InvalidAttributeNameException) SerializationHelper.Deserialize(SerializationHelper.Serialize(ex)); },
				Throws.Nothing);
			Assert.That(dex, Is.Not.Null);
			Assert.That(dex.AttributeName, Is.EqualTo("blah"));
			Assert.That(dex.Class, Is.EqualTo(typeof(ExceptionSerializationFixture)));
		}

		[Test]
		public void InvalidPropertyNameException()
		{
			var ex = new InvalidPropertyNameException("blah", typeof(ExceptionSerializationFixture));
			var dex = default(InvalidPropertyNameException);
			Assert.That(
				() => { dex = (InvalidPropertyNameException) SerializationHelper.Deserialize(SerializationHelper.Serialize(ex)); },
				Throws.Nothing);
			Assert.That(dex, Is.Not.Null);
			Assert.That(dex.PropertyName, Is.EqualTo("blah"));
			Assert.That(dex.Class, Is.EqualTo(typeof(ExceptionSerializationFixture)));
		}

		[Test]
		public void InvalidStateException()
		{
			TestsContext.AssumeSystemTypeIsSerializable();
			var state = new[] { new InvalidValue("blah", typeof(ExceptionSerializationFixture), "prop", null, null, null) };
			var ex = new InvalidStateException(state);
			var dex = default(InvalidStateException);
			Assert.That(
				() => { dex = (InvalidStateException) SerializationHelper.Deserialize(SerializationHelper.Serialize(ex)); },
				Throws.Nothing);
			Assert.That(dex, Is.Not.Null);
			Assert.That(dex.InvalidValues, Has.Length.EqualTo(1));
			Assert.That(
				dex.InvalidValues[0],
				Is.Not.Null.And.Property(nameof(InvalidValue.Message)).EqualTo("blah"));
			Assert.That(dex.InvalidValues[0].EntityType, Is.EqualTo(typeof(ExceptionSerializationFixture)));
			Assert.That(dex.InvalidValues[0].PropertyName, Is.EqualTo("prop"));
		}
	}
}
