using System.Linq;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Specifics.NHV25
{
	[TestFixture]
	public class DictionaryValueKeyFixture : BaseValidatorFixture
	{
		[Test]
		public void DictionaryEnumKey()
		{
			IClassValidator validator = GetClassValidator(typeof (Tv));

			var tv = new Tv();
			tv.name = "France 2";
			var showOk = new Show();
			showOk.name = "Tout le monde en parle";
			var showNok = new Show();
			showNok.name = null;

			// Fails with Enum type key
			tv.showse.Add(TestEnum.uno, showOk);
			tv.showse.Add(TestEnum.due, showNok);
			tv.showse.Add(TestEnum.tre, null);
			validator.GetInvalidValues(tv).Should().Not.Be.Empty();
		}

		/// <summary>
		/// Validate Generic Dictionaries with ValueKey
		/// </summary>
		[Test]
		public void DictionaryIntKey()
		{
			IClassValidator validator = GetClassValidator(typeof (Tv));

			// Test pass with int type key
			var tv = new Tv();
			tv.name = "France 2";
			var showOk = new Show();
			showOk.name = "Tout le monde en parle";
			var showNok = new Show();
			showNok.name = null;
			tv.shows.Add(1, showOk);
			tv.shows.Add(2, showNok);
			tv.shows.Add(3, null);
			var invalidValues = validator.GetInvalidValues(tv);
			invalidValues.Should().Not.Be.Empty();
			invalidValues.Single().PropertyPath.Should().Be.EqualTo("shows[2].name");
		}
	}
}