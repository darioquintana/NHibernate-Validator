using System.Collections;
using System.Collections.Generic;
using NHibernate.Collection.Generic;
using NUnit.Framework;

namespace NHibernate.Validator.Tests.DeepIntegration
{
	[TestFixture]
	public class MultipleBagFetchFixture : AbstractMultipleCollectionFixture
	{
		protected override IList Mappings
		{
			get { return new string[] { "DeepIntegration.PersonBag.hbm.xml" }; }
		}

		protected override void AddToCollection(ICollection<Person> collection, Person person)
		{
			PersistentGenericBag<Person> concrete = collection as PersistentGenericBag<Person>;
			if (concrete != null)
				concrete.Add(person);
			else
				((ArrayList)collection).Add(person);
		}

		protected override ICollection<Person> CreateCollection()
		{
			return new List<Person>();
		}
	}
}