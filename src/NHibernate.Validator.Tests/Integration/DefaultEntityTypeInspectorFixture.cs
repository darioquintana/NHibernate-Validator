using System.Collections;
using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Integration
{
	[TestFixture]
	public class DefaultEntityTypeInspectorFixture : PersistenceTest
	{
		protected override IList Mappings
		{
			get { return new[] { "Integration.AnEntity.hbm.xml" }; }
		}

		[Test]
		public void DoesNotRecognizeConcreteType()
		{
			var ei = new DefaultEntityTypeInspector();
			Assert.That(ei.GuessType(new object()), Is.Null);
			Assert.That(ei.GuessType(new AnEntity()), Is.Null);
		}

		[Test]
		public void ReturnTheCorrectType()
		{
			AnEntity proxy;
			using (ISession s = OpenSession())
			{
				proxy = s.Load<AnEntity>(1);
			}
			var ei = new DefaultEntityTypeInspector();
			Assert.That(ei.GuessType(proxy), Is.EqualTo(typeof (AnEntity)));
			Assert.That(!NHibernateUtil.IsInitialized(proxy), "should not initialize the proxy");
		}
	}
}