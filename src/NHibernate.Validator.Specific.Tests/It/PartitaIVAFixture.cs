using System.Linq;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Specific.It;
using NHibernate.Validator.Tests;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Specific.Tests.It
{
	/// <summary>
	/// Le Partite IVA possono essere verificate su
	/// http://www1.agenziaentrate.it/servizi/vies/vies.htm?act=step1&stato=IT&piva=&ses=1215770418#app
	/// </summary>
	[TestFixture]
	public class PartitaIVAFixture : BaseValidatorFixture
	{
		[Test]
		public void IsValid()
		{
			PartitaIvaValidator v = new PartitaIvaValidator();
			Assert.IsTrue(v.IsValid("01636120634", null));
			Assert.IsTrue(v.IsValid("01463240745", null));
			Assert.IsTrue(v.IsValid("00548830678", null));
			Assert.IsTrue(v.IsValid("02599560758", null));
			Assert.IsTrue(v.IsValid("00738730654", null));
			Assert.IsTrue(v.IsValid("00523580520", null));
			Assert.IsTrue(v.IsValid("00711710764", null));
			Assert.IsTrue(v.IsValid(711710764, null));
			Assert.IsTrue(v.IsValid(null, null));
			Assert.IsTrue(v.IsValid(string.Empty, null));

			Assert.IsFalse(v.IsValid("1234567890123", null));
			Assert.IsFalse(v.IsValid("A0007016322", null));
			Assert.IsFalse(v.IsValid("000A7016322", null));

			Assert.IsFalse(v.IsValid("00007016322", null));
			Assert.IsFalse(v.IsValid(7016322, null));
			Assert.IsFalse(v.IsValid("00015206543", null));
			Assert.IsFalse(v.IsValid("00224546291", null));
			Assert.IsFalse(v.IsValid("00322088352", null));
			Assert.IsFalse(v.IsValid("38388669632", null));
			Assert.IsFalse(v.IsValid("62394317077", null));
			Assert.IsFalse(v.IsValid("95317556041", null));
		}

		[Test]
		public void ValidatorAttribute()
		{
			IClassValidator userValidator = GetClassValidator(typeof(Cliente));
			Cliente c = new Cliente();
			userValidator.GetInvalidValues(c).Should().Be.Empty();
			c.Piva = "00007016322";
			var iv = userValidator.GetInvalidValues(c);
			iv.Should().Not.Be.Empty();
			iv.Single().Message.Should().Be.EqualTo("Partita IVA incorretta.");
			c.Piva = "00523580520";
			userValidator.GetInvalidValues(c).Should().Be.Empty();
		}

		public class Cliente
		{
			[PartitaIva]
			private string pivaField = "01636120634";
			private string piva;

			[PartitaIva]
			public string Piva
			{
				get { return piva; }
				set { piva = value; }
			}
		}
	}
}