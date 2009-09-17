using System;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Engine
{
	[TestFixture]
	public class MultiEntityTypeInspectorFixture
	{
		private class EntityTypeInspectorStub : IEntityTypeInspector
		{
			private readonly System.Type returnValue;

			public EntityTypeInspectorStub(System.Type returnValue)
			{
				this.returnValue = returnValue;
			}

			#region Implementation of IEntityTypeInspector

			public System.Type GuessType(object entityInstance)
			{
				return returnValue;
			}

			#endregion
		}

		[Test]
		public void Ctor()
		{
			Assert.Throws<ArgumentNullException>(() => new MultiEntityTypeInspector(null));
			Assert.DoesNotThrow(() => new MultiEntityTypeInspector(new IEntityTypeInspector[0]));
		}

		[Test]
		public void ReturnFirstValid()
		{
			var expected = typeof (MultiEntityTypeInspectorFixture);
			var meti =
				new MultiEntityTypeInspector(new[]
				                             	{new EntityTypeInspectorStub(expected), new EntityTypeInspectorStub(typeof (object))});
			Assert.That(meti.GuessType(5), Is.EqualTo(expected));

			expected = typeof (object);
			meti =
				new MultiEntityTypeInspector(new[] {new EntityTypeInspectorStub(null), new EntityTypeInspectorStub(typeof (object))});
			Assert.That(meti.GuessType(5), Is.EqualTo(expected));
		}

		[Test]
		public void ReturnNull()
		{
			var meti =
				new MultiEntityTypeInspector(new[] { new EntityTypeInspectorStub(null), new EntityTypeInspectorStub(null) });
			Assert.That(meti.GuessType(5), Is.Null);
		}
	}
}