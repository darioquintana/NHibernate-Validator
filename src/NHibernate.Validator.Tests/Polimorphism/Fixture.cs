using NUnit.Framework;

namespace NHibernate.Validator.Tests.Polimorphism
{
	[TestFixture, Ignore("Need polimorphism behavior. See this post:http://blogs.msdn.com/tomholl/archive/2008/03/02/polymorphism-and-the-validation-application-block.aspx")]
	public class PolimorphismFixture
	{
		[Test]
		public void DoPolimorphism()
		{
			DerivatedClass d = new DerivatedClass();
			d.A = "hola";
			d.B = "hola";
			
			ClassValidator vtor = new ClassValidator(typeof(DerivatedClass));
			InvalidValue[] values = vtor.GetInvalidValues(d);
			Assert.AreEqual(2,values.Length);
			
			ClassValidator vtor2 = new ClassValidator(typeof(BaseClass));
			InvalidValue[] values2 = vtor2.GetInvalidValues(d);
			Assert.AreEqual(2, values2.Length,"Need support of Polimorphism");
		}
	}
}