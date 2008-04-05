using NUnit.Framework;

namespace NHibernate.Validator.Tests.Polimorphism
{
	[TestFixture]
	public class PolimorphismFixture
	{
		///See this post http://blogs.msdn.com/tomholl/archive/2008/03/02/polymorphism-and-the-validation-application-block.aspx"

		[Test]
		public void DoPolimorphismWithClasses()
		{
			DerivatedClass d = new DerivatedClass();
			d.A = "hola";
			d.B = "hola";
			
			ClassValidator vtor = new ClassValidator(typeof(DerivatedClass));
			InvalidValue[] values = vtor.GetInvalidValues(d);
			Assert.AreEqual(2,values.Length);
			
			ClassValidator vtor2 = new ClassValidator(typeof(BaseClass));
			InvalidValue[] values2 = vtor2.GetInvalidValues(d);
			Assert.AreEqual(2, values2.Length, "Polimorphic support is no working");
		}

		[Test]
		public void DoPolimorphismWithInterfaces()
		{
			Impl obj = new Impl();
			obj.A = "hola";
			obj.B = "hola";

			ClassValidator vtor = new ClassValidator(typeof(Impl));
			InvalidValue[] values = vtor.GetInvalidValues(obj);
			Assert.AreEqual(2,values.Length);

			ClassValidator vtor2 = new ClassValidator(typeof(IContract));
			InvalidValue[] values2 = vtor2.GetInvalidValues(obj);
			Assert.AreEqual(2, values2.Length, "Polimorphic support is no working");
		}

		/// <summary>
		/// This test if the internal storage of ClassValidator works.
		/// </summary>
		[Test]
		public void StorageOfClassValidators()
		{
			DerivatedClass d1 = new DerivatedClass();
			d1.A = "hola";
			d1.B = "hola";

			DerivatedClass d2 = new DerivatedClass();
			d2.A = "hola";
			d2.B = "hola";

			IClassValidator vtor = new ClassValidator(typeof (BaseClass));

			Assert.AreEqual(2,vtor.GetInvalidValues(d1).Length);
			Assert.AreEqual(2,vtor.GetInvalidValues(d2).Length);
		}
	}
}