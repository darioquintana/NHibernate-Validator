using System.Collections;
using System.Collections.Generic;
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
			((ArrayList) collection).Add(person);
		}

		protected override ICollection CreateCollection()
		{
			return new ArrayList();
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