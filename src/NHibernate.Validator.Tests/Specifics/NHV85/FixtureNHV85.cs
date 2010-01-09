using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NHibernate.Validator.Tests.DeepIntegration;
using System.Collections;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Event;

namespace NHibernate.Validator.Tests.Specifics.NHV85
{
    [TestFixture]
    public class FixtureNHV85 : PersistenceTest
    {
        protected override IList Mappings
        {
            get { return new string[] { "Specifics.NHV85.Model.hbm.xml" }; }
        }

        [Test]
        public void When_adding_transient_object_to_uninitialized_collection_should_validate_that_object()
        {
            Parent p = new Parent("x");
            p.Children.Add(new Child("kik"));
            using (var s = OpenSession())
            using (ITransaction tx = s.BeginTransaction())
            {
                s.SaveOrUpdate(p);
                tx.Commit();
            }

            Parent loadedParent;
            using (var s = OpenSession())
            {
                loadedParent = s.CreateQuery("from Parent p where p.Name = 'x'")
					.UniqueResult<Parent>();

                loadedParent.Children.Add(new Child("y"));

                Assert.IsFalse(vengine.IsValid(loadedParent));
            }
        }

        [TearDown]
        public void CleanUp()
        {
            using (ISession s = OpenSession())
            using (ITransaction t = s.BeginTransaction())
            {
                s.Delete("from Parent p");
                s.Delete("from Child c");
                t.Commit();
            }
        }

        protected ValidatorEngine vengine;
        protected override void Configure(NHibernate.Cfg.Configuration configuration)
        {
            Environment.SharedEngineProvider = new NHibernateSharedEngineProvider();
            vengine = NHibernate.Validator.Cfg.Environment.SharedEngineProvider.GetEngine();
            vengine.Clear();
            XmlConfiguration nhvc = new XmlConfiguration();
            nhvc.Properties[Environment.ApplyToDDL] = "false";
            nhvc.Properties[Environment.AutoregisterListeners] = "true";
            nhvc.Properties[Environment.ValidatorMode] = "UseAttribute";
            vengine.Configure(nhvc);

            ValidatorInitializer.Initialize(configuration);
        }
    }
}
