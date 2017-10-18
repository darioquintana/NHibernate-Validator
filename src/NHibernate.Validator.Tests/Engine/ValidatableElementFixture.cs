using System;
using NHibernate.Properties;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.Engine
{
	[TestFixture]
	public class ValidatableElementFixture
	{
		[Test]
		public void DefCtor()
		{
			Assert.That(() => new ValidatableElement(null, null), Throws.TypeOf<ArgumentNullException>());
			Assert.That(() => new ValidatableElement(typeof(Address), null), Throws.TypeOf<ArgumentNullException>());
		}

		[Test]
		public void ValidatableElementTest()
		{
			ClassValidator cv = new ClassValidator(typeof(Address));
			ValidatableElement ve = new ValidatableElement(typeof(Address), cv);
			Assert.AreEqual(ve.EntityType, typeof(Address));
			Assert.IsTrue(ReferenceEquals(cv, ve.Validator));
			Assert.IsNull(ve.Getter);
			Assert.IsFalse(ve.SubElements.GetEnumerator().MoveNext());
			Assert.IsTrue(ve.Equals(new ValidatableElement(typeof(Address), new ClassValidator(typeof(Address)))));
			Assert.IsFalse(ve.Equals(5)); // any other obj
			Assert.AreEqual(ve.GetHashCode(),
											(new ValidatableElement(typeof(Address), new ClassValidator(typeof(Address)))).GetHashCode());
		}

		public class AClass
		{
			private Address address;

			public Address Address
			{
				get { return address; }
				set { address = value; }
			}

		}

		[Test]
		public void SubElements()
		{
			IGetter getter = new BasicPropertyAccessor().GetGetter(typeof(AClass), "Address");
			ClassValidator cvadd = new ClassValidator(typeof(Address));
			ClassValidator cv = new ClassValidator(typeof(AClass));
			ValidatableElement ve = new ValidatableElement(typeof(AClass), cv);
			try
			{
				ve.AddSubElement(new ValidatableElement(typeof(Address), cvadd));
				Assert.Fail("No exception adding a subelement without getter");
			}
			catch (ArgumentException)
			{
				//ok
			}
			Assert.IsFalse(ve.HasSubElements);
			ve.AddSubElement(new ValidatableElement(typeof(Address), cvadd, getter));
			Assert.IsTrue(ve.HasSubElements);
		}
	}
}
