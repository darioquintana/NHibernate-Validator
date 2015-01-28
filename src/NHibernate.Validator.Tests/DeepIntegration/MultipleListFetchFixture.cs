using System.Collections;
using System.Collections.Generic;
using NHibernate.Collection;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.DeepIntegration
{
	[TestFixture]
	public class MultipleListFetchFixture : AbstractMultipleCollectionFixture
	{
		protected override IList Mappings
		{
			get { return new string[] { "DeepIntegration.PersonList.hbm.xml" }; }
		}

		protected override void AddToCollection(ICollection collection, Person person)
		{
			var concrete = collection as List<Person>;
			if (concrete != null)
				concrete.Add(person);
			else
				((ArrayList)collection).Add(person);
		}

		protected override ICollection<Person> CreateCollection()
		{
			return new List<Person>();
		}

		protected override void AddToCollection(ICollection<Person> collection, Person person)
		{
			collection.Add(person);
		}

		protected override ICollection<Person> GCreateCollection()
		{
			return new List<Person>();
		}
	}
}