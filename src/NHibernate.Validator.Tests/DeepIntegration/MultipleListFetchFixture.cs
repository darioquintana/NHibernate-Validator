using System.Collections;
using System.Collections.Generic;
using NHibernate.Collection.Generic;
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

		protected override ICollection<Person> CreateCollection()
		{
			return new List<Person>();
		}

		protected override void AddToCollection(ICollection<Person> collection, Person person)
		{
			collection.Add(person);
		}
	}
}