using NHibernate.Validator.Engine;
using NUnit.Framework;

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
			InvalidValue[] valuese = validator.GetInvalidValues(tv);
			Assert.AreEqual(1, valuese.Length);
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
			InvalidValue[] values = validator.GetInvalidValues(tv);
			Assert.AreEqual(1, values.Length);
			Assert.AreEqual("shows[2].name", values[0].PropertyPath);
		}
	}
}