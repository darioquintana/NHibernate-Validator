using NHibernate.Validator.Engine;
using NHibernate.Validator.Specific.It;
using NHibernate.Validator.Tests;
using NUnit.Framework;

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
			Assert.IsTrue(v.IsValid("01636120634"));
			Assert.IsTrue(v.IsValid("01463240745"));
			Assert.IsTrue(v.IsValid("00548830678"));
			Assert.IsTrue(v.IsValid("02599560758"));
			Assert.IsTrue(v.IsValid("00738730654"));
			Assert.IsTrue(v.IsValid("00523580520"));
			Assert.IsTrue(v.IsValid("00711710764"));
			Assert.IsTrue(v.IsValid(711710764));
			Assert.IsTrue(v.IsValid(null));

			Assert.IsFalse(v.IsValid(""));
			Assert.IsFalse(v.IsValid("1234567890123"));
			Assert.IsFalse(v.IsValid("A0007016322"));
			Assert.IsFalse(v.IsValid("000A7016322"));

			Assert.IsFalse(v.IsValid("00007016322"));
			Assert.IsFalse(v.IsValid(7016322));
			Assert.IsFalse(v.IsValid("00015206543"));
			Assert.IsFalse(v.IsValid("00224546291"));
			Assert.IsFalse(v.IsValid("00322088352"));
			Assert.IsFalse(v.IsValid("38388669632"));
			Assert.IsFalse(v.IsValid("62394317077"));
			Assert.IsFalse(v.IsValid("95317556041"));
		}

		[Test]
		public void ValidatorAttribute()
		{
			ClassValidator userValidator = GetClassValidator(typeof(Cliente));
			Cliente c = new Cliente();
			Assert.AreEqual(0, userValidator.GetInvalidValues(c).Length);
			c.Piva = "00007016322";
			InvalidValue[] iv = userValidator.GetInvalidValues(c);
			Assert.AreEqual(1, iv.Length);
			Assert.AreEqual("Partita IVA incorretta.", iv[0].Message);
			c.Piva = "00523580520";
			Assert.AreEqual(0, userValidator.GetInvalidValues(c).Length);
		}
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