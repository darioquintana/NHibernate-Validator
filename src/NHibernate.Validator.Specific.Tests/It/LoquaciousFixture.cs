using System.Linq;
using System.Reflection;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Mappings;
using NHibernate.Validator.Specific.It;
using NUnit.Framework;

namespace NHibernate.Validator.Specific.Tests.It
{
	[TestFixture]
	public class LoquaciousFixture
	{
		private const BindingFlags membersBindingFlags =
			BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
			| BindingFlags.Static;

		[Test]
		public void ShouldWorkCodiceFiscaleExtensions()
		{
			var v = new ValidationDef<Cliente>();
			const string expectedMessage = "Codice fiscale non valido";
			v.Define(x => x.CodiceFiscale).IsCodiceFiscale().WithMessage(expectedMessage);
			IClassMapping cm = ((IMappingSource)v).GetMapping();
			PropertyInfo pi = typeof (Cliente).GetProperty("CodiceFiscale", membersBindingFlags);
			Assert.That(cm.GetMemberAttributes(pi).Count(), Is.EqualTo(1));
			CodiceFiscaleAttribute first = cm.GetMemberAttributes(pi).OfType<CodiceFiscaleAttribute>().FirstOrDefault();
			Assert.That(first, Is.Not.Null);
			Assert.That(first.Message, Is.EqualTo(expectedMessage));
		}

		[Test]
		public void ShouldWorkPartitaIvaExtensions()
		{
			const string expectedMessage = "Partita IVA non valida";
			var v = new ValidationDef<Cliente>();
			v.Define(x => x.Piva).NotNullable().And.IsPartitaIva().WithMessage(expectedMessage);
			PropertyInfo pi = typeof (Cliente).GetProperty("Piva", membersBindingFlags);
			IClassMapping cm = ((IMappingSource)v).GetMapping();
			Assert.That(cm.GetMemberAttributes(pi).Count(), Is.EqualTo(2));
			PartitaIvaAttribute pa = cm.GetMemberAttributes(pi).OfType<PartitaIvaAttribute>().FirstOrDefault();
			Assert.That(pa, Is.Not.Null);
			Assert.That(pa.Message, Is.EqualTo(expectedMessage));
		}

		public class Cliente
		{
			public string CodiceFiscale { get; set; }
			public string Piva { get; set; }
		}
	}
}