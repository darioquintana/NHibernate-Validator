using System.Linq;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Polimorphism
{
	[TestFixture]
	public class PolimorphismFixture
	{
		//See this post http://blogs.msdn.com/tomholl/archive/2008/03/02/polymorphism-and-the-validation-application-block.aspx"
		// NHV ClassValidator can be used without generics this mean that the right way to create a new 
		// ClassValidator is in presence of polimorphic classes is : new ClassValidator(entity.GetType())
		// Using the ValidatorEngine you can don't worry about that ;)

		[Test]
		public void DoPolimorphismWithClasses()
		{
			DerivatedClass d = new DerivatedClass();
			d.A = "hola";
			d.B = "hola";
			
			ClassValidator vtor = new ClassValidator(typeof(DerivatedClass));
			vtor.GetInvalidValues(d).Should().Have.Count.EqualTo(2);
			
			ClassValidator vtor2 = new ClassValidator(typeof(BaseClass));
			vtor2.GetInvalidValues(d).Should("Polimorphic support is no working").Have.Count.EqualTo(1);

			ValidatorEngine ve = new ValidatorEngine();
			ve.Validate(d).Should().Have.Count.EqualTo(2);
		}

		[Test]
		public void DoPolimorphismWithInterfaces()
		{
			Impl obj = new Impl();
			obj.A = "hola";
			obj.B = "hola";

			ClassValidator vtor = new ClassValidator(typeof(Impl));
			vtor.GetInvalidValues(obj).Should().Have.Count.EqualTo(2);

			ClassValidator vtor2 = new ClassValidator(typeof(IContract));
			vtor2.GetInvalidValues(obj).Should("Polimorphic support is no working").Have.Count.EqualTo(1);

			ValidatorEngine ve = new ValidatorEngine();
			ve.Validate(obj).Should().Have.Count.EqualTo(2);
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
			Composition c = new Composition();

			c.Any = bBroken;
			vtor.GetInvalidValues(c).Should().Have.Count.EqualTo(1);
			
			c.Any = dFullBroken;
			vtor.GetInvalidValues(c).Should().Have.Count.EqualTo(2);

			c.Any = bOk;
			vtor.GetInvalidValues(c).Should().Be.Empty();

			c.Any = dPartialBroken;
			var ivalue = vtor.GetInvalidValues(c);
			ivalue.Should().Not.Be.Empty();
			ivalue.Single().PropertyName.Should().Be.EqualTo("A");
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
			var ivalue = ve.Validate(c);
			ivalue.Should().Not.Be.Empty();
			ivalue.Single().PropertyName.Should().Be.EqualTo("A");
		}
	}
}