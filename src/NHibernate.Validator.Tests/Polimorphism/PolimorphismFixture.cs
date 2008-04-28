using NHibernate.Validator.Engine;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Polimorphism
{
	[TestFixture]
	public class PolimorphismFixture
	{
		//See this post http://blogs.msdn.com/tomholl/archive/2008/03/02/polymorphism-and-the-validation-application-block.aspx"
		// NHV ClassValidator can be used without generics this mean that the right way to create a new 
		// ClassValidator is in presence of polimorphic classes is : new ClassValidator(bean.GetType())
		// Using the ValidatorEngine you can don't wary about that ;)

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
			Assert.AreEqual(1, values2.Length, "Polimorphic support is no working");

			ValidatorEngine ve = new ValidatorEngine();
			values = ve.Validate(d);
			Assert.AreEqual(2, values.Length);
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
			Assert.AreEqual(1, values2.Length, "Polimorphic support is no working");

			ValidatorEngine ve = new ValidatorEngine();
			values = ve.Validate(obj);
			Assert.AreEqual(2, values.Length);
		}

		[Test]
		public void Coposition()
		{
			DerivatedClass dFullBroken = new DerivatedClass();
			dFullBroken.A = "1234";
			dFullBroken.B = "1234";

			DerivatedClass dPartialBroken = new DerivatedClass();
			dPartialBroken.A = "1234";

			BaseClass bOk = new BaseClass();
			bOk.A = "123";

			BaseClass bBroken = new BaseClass();
			bBroken.A = "1234";

			ClassValidator vtor = new ClassValidator(typeof(Composition));
			InvalidValue[] ivalues;
			Composition c = new Composition();

			c.Any = bBroken;
			ivalues = vtor.GetInvalidValues(c);
			Assert.AreEqual(1, ivalues.Length);

			c.Any = dFullBroken;
			ivalues = vtor.GetInvalidValues(c);
			Assert.AreEqual(2, ivalues.Length);

			c.Any = bOk;
			ivalues = vtor.GetInvalidValues(c);
			Assert.AreEqual(0, ivalues.Length);

			c.Any = dPartialBroken;
			ivalues = vtor.GetInvalidValues(c);
			Assert.AreEqual(1, ivalues.Length);
			Assert.AreEqual("A", ivalues[0].PropertyName);
		}

		[Test]
		public void CopositionUsingEngine()
		{
			DerivatedClass dFullBroken = new DerivatedClass();
			dFullBroken.A = "1234";
			dFullBroken.B = "1234";

			DerivatedClass dPartialBroken = new DerivatedClass();
			dPartialBroken.A = "1234";

			BaseClass bOk = new BaseClass();
			bOk.A = "123";

			BaseClass bBroken = new BaseClass();
			bBroken.A = "1234";

			ValidatorEngine ve = new ValidatorEngine();
			InvalidValue[] ivalues;
			Composition c = new Composition();

			c.Any = bBroken;
			ivalues = ve.Validate(c);
			Assert.AreEqual(1, ivalues.Length);

			c.Any = dFullBroken;
			ivalues = ve.Validate(c);
			Assert.AreEqual(2, ivalues.Length);

			c.Any = bOk;
			ivalues = ve.Validate(c);
			Assert.AreEqual(0, ivalues.Length);

			c.Any = dPartialBroken;
			ivalues = ve.Validate(c);
			Assert.AreEqual(1, ivalues.Length);
			Assert.AreEqual("A", ivalues[0].PropertyName);
		}
	}
}