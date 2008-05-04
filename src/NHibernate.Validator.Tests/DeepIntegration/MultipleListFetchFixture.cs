using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Validator.Tests.DeepIntegration;
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
			throw new NotImplementedException();
		}

		protected override ICollection<Person> GCreateCollection()
		{
			throw new NotImplementedException();
		}
	}
}