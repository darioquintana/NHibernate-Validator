using NHibernate.Validator.Engine;
using NHibernate.Validator.Specific.Uy;
using NHibernate.Validator.Tests;
using NUnit.Framework;

namespace NHibernate.Validator.Specific.Tests.Uy
{
	/// <summary>
	/// El algoritmo para validar una cedula uruguaya puede encontrarse en:
	/// http://es.wikipedia.org/wiki/DNI#Uruguay
	/// </summary>
	[TestFixture]
	public class CedulaIdentidadFixture : BaseValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			CedulaIdentidadValidator v = new CedulaIdentidadValidator();
			Assert.IsTrue(v.IsValid("27526962"));
			Assert.IsTrue(v.IsValid("27529390"));
			Assert.IsTrue(v.IsValid(11748364));
			Assert.IsTrue(v.IsValid("10092166"));
			Assert.IsTrue(v.IsValid(null));
			Assert.IsTrue(v.IsValid(string.Empty));

			Assert.IsFalse(v.IsValid("1234567890123"));
			Assert.IsFalse(v.IsValid(701632));
			Assert.IsFalse(v.IsValid(12345678));
		}

		[Test]
		public void ValidatorAttribute()
		{
			IClassValidator userValidator = GetClassValidator(typeof(Persona));
			Persona p = new Persona();
			Assert.AreEqual(0, userValidator.GetInvalidValues(p).Length);
			p.Cedula = "44443333";
			InvalidValue[] iv = userValidator.GetInvalidValues(p);
			Assert.AreEqual(1, iv.Length);
			Assert.AreEqual("Cedula de identidad incorrecta.", iv[0].Message);
			p.Cedula = "11748364";
			Assert.AreEqual(0, userValidator.GetInvalidValues(p).Length);
		}

		public class Persona
		{
			[CedulaIdentidad]
			private string cedulaField = "27526962";
			private string cedula;

			[CedulaIdentidad]
			public string Cedula
			{
				get { return cedula; }
				set { cedula = value; }
			}
		}
	}
}