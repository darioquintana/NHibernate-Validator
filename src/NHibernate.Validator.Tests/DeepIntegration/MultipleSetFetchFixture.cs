using System.Collections;
using System.Collections.Generic;
using Iesi.Collections;
using Iesi.Collections.Generic;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.DeepIntegration
{
	[TestFixture]
	public class MultipleSetFetchFixture : AbstractMultipleCollectionFixture
	{
		protected override IList Mappings
		{
			get { return new string[] { "DeepIntegration.PersonSet.hbm.xml" }; }
		}

		protected override void AddToCollection(ICollection collection, Person person)
		{
			((ISet<Person>) collection).Add(person);
		}

		protected override ICollection<Person> CreateCollection()
		{
			return new HashSet<Person>();
		}

		protected override void AddToCollection(ICollection<Person> collection, Person person)
		{
			collection.Add(person);
		}

		protected override ICollection<Person> GCreateCollection()
		{
			return new HashSet<Person>();
		}
	}
}