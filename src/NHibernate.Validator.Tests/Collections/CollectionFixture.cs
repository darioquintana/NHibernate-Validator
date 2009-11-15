using System.Linq;
using NHibernate.Validator.Engine;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Collections
{
	using System.Collections.Generic;
	using NUnit.Framework;

	[TestFixture]
	public class CollectionFixture : BaseValidatorFixture
	{
		/// <summary>
		/// Validate Generic Collections
		/// </summary>
		[Test]
		public void Collection()
		{
			Tv tv = new Tv();
			tv.name = "France 2";
			Presenter presNok = new Presenter();
			presNok.name = null;
			Presenter presOk = new Presenter();
			presOk.name = "Thierry Ardisson";
			tv.presenters.Add(presOk);
			tv.presenters.Add(presNok);
			IClassValidator validator = GetClassValidator(typeof(Tv));

			var values = validator.GetInvalidValues(tv);
			values.Should().Not.Be.Empty();
			values.Single().PropertyPath.Should().Be.EqualTo("presenters[1].name");
			tv.presenters.Clear();

			tv.dontNeedDeepValidation = new List<string>();
			tv.dontNeedDeepValidation.Add("something");
			values = validator.GetInvalidValues(tv);
			values.Should().Be.Empty();
			tv.dontNeedDeepValidation.Add("something else");
			values = validator.GetInvalidValues(tv);
			values.Should().Not.Be.Empty();
			values.Single().PropertyPath.Should().Be.EqualTo("dontNeedDeepValidation");
		}

		/// <summary>
		/// Validate Generic Dictionaries
		/// </summary>
		[Test]
		public void Dictionary()
		{
			IClassValidator validator = GetClassValidator(typeof(Tv));

			Tv tv = new Tv();
			tv.name = "France 2";
			Show showOk = new Show();
			showOk.name = "Tout le monde en parle";
			Show showNok = new Show();
			showNok.name = null;
			tv.shows.Add("Midnight", showOk);
			tv.shows.Add("Primetime", showNok);
			tv.shows.Add("Nothing", null);
			var values = validator.GetInvalidValues(tv);
			values.Should().Not.Be.Empty();
			values.Single().PropertyPath.Should().Be.EqualTo("shows[Primetime].name");

			//Inverted dictionary
			tv = new Tv();
			tv.name = "France 2";
			tv.validatableInKey = new Dictionary<Simple, string>();
			tv.validatableInKey.Add(new Simple("Exalibur"), "Coll1");
			tv.validatableInKey.Add(new Simple(), "Coll2");
			values = validator.GetInvalidValues(tv);
			values.Should().Not.Be.Empty();
			values.Single().PropertyPath.Should().Be.EqualTo("validatableInKey[null].name");
		}

		/// <summary>
		/// Validate Arrays
		/// </summary>
		[Test]
		public void Array()
		{
			Tv tv = new Tv();
			tv.name = "France 2";
			Movie movieOk = new Movie();
			movieOk.Name = "Kill Bill";
			Movie movieNok = new Movie();
			movieNok.Name = null;
			tv.movies = new Movie[] {movieOk, null, movieNok};
			IClassValidator validator = GetClassValidator(typeof(Tv));
			var values = validator.GetInvalidValues(tv);
			values.Should().Not.Be.Empty();
			values.Single().PropertyPath.Should().Be.EqualTo("movies[2].Name");
		}

		[Test]
		public void Size()
		{
			HasCollection hc = new HasCollection();
			IClassValidator vtor = GetClassValidator(typeof(HasCollection));

			hc.StringCollection = new List<string>(new string[] {"cuidado", "con", "el", "carancho!"});
			vtor.GetInvalidValues(hc).Should().Be.Empty();
			
			hc.StringCollection = new List<string>(new string[] { "pombero" });
			vtor.GetInvalidValues(hc).Should().Not.Be.Empty();
			
			hc.StringCollection = new List<string>(new string[] { "Los","Pekes","Chicho","Nino","Nono","Fito" });
			vtor.GetInvalidValues(hc).Should().Not.Be.Empty();
		}

		[Test]
		public void SizeWithValid()
		{
			HasShowCollection hsc = new HasShowCollection();
			IClassValidator vtor = GetClassValidator(typeof(HasShowCollection));

			hsc.Shows = new List<Show>( new Show[] {new Show("s1"), new Show("s2")} );
			vtor.GetInvalidValues(hsc).Should().Be.Empty();

			hsc.Shows = new List<Show>(new Show[] { new Show("s1")});
			vtor.GetInvalidValues(hsc).Should().Have.Count.EqualTo(1);

			hsc.Shows = new List<Show>(new Show[] { new Show(null) });
			vtor.GetInvalidValues(hsc).Should().Have.Count.EqualTo(2);

			hsc.Shows = new List<Show>(new Show[] { new Show("s1"), new Show("s2"), new Show("s3"), new Show("s3") });
			vtor.GetInvalidValues(hsc).Should().Have.Count.EqualTo(1);

			hsc.Shows = new List<Show>(new Show[] { new Show(null), new Show("s2"), new Show("s3"), new Show("s3") });
			vtor.GetInvalidValues(hsc).Should().Have.Count.EqualTo(2);
		}

		[Test]
		public void SizeArrayWithValid()
		{
			HasArrayWithValid hsc = new HasArrayWithValid();
			IClassValidator vtor = GetClassValidator(typeof(HasArrayWithValid));

			hsc.Shows = new Show[] {new Show("s1"), new Show("s2")};
			vtor.GetInvalidValues(hsc).Should().Be.Empty();

			hsc.Shows = new Show[] { new Show("s1"), new Show(null) };
			vtor.GetInvalidValues(hsc).Should().Not.Be.Empty();

			hsc.Shows = new Show[] { new Show("s1"), new Show("s2"), new Show("s3"), new Show("s4"),new Show("s5") };
			vtor.GetInvalidValues(hsc).Should().Have.Count.EqualTo(1);
		}
	}
}