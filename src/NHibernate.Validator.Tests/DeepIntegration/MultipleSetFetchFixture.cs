using System;
using System.Collections;
using System.Collections.Generic;
using Iesi.Collections;
using NHibernate.Validator.Tests.DeepIntegration;
using NUnit.Framework;
using Iesi.Collections.Generic;

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
			((ISet) collection).Add(person);
		}

		protected override ICollection CreateCollection()
		{
			return new HashedSet();
		}

		protected override void AddToCollection(ICollection<Person> collection, Person person)
		{
			((ISet<Person>) collection).Add(person);
		}

		protected override ICollection<Person> GCreateCollection()
		{
			return new HashedSet<Person>();
		}
	}
}