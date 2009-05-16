using System;
using NHibernate.Cfg;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Demo.Model;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;

namespace NHibernate.Validator.Demo.NHI
{
	internal class Program
	{
		private static void Main()
		{
			/// In this example we are showing how to easily integrate 
			/// NHibernate Validator with the NHibernate persistence service.
			
			TryToSaveAnInvalidEntity();

			SavingAValidEntity();

			Console.WriteLine("Done. Everything worked as expected.");
			Console.ReadLine();
		}

		/// <summary>
		/// On this example we attempt to save an invalid entity. 
		/// As you can see the NHibernate.Validator integration with 
		/// NHibernate will throw an InvalidStateException exception,
		/// and no hit will make to the database.
		/// </summary>
		private static void TryToSaveAnInvalidEntity()
		{
			Console.WriteLine("======================================");
			Console.WriteLine("Example: Try to save an invalid entity");
			Console.WriteLine("======================================");

			//NHibernate configuration: see hibernate.cfg.xml
			Configuration cfg = new Configuration();
			cfg.Configure();

			//NHibernate Validator configuration: see nhvalidator.cfg.xml
			ValidatorEngine engine = new ValidatorEngine();
			engine.Configure(); //by convention reads the nhvalidator.cfg.xml file.

			//Registering of Listeners and DDL-applying here
			ValidatorInitializer.Initialize(cfg, engine);

			ISessionFactory sf = cfg.BuildSessionFactory();

			Customer customer = new Customer();
			customer.Born = DateTime.Parse("1/1/1982");
			customer.Email = "antonio@mail.com";
			customer.FirstName = "Antonio";
			customer.LastName = "Gonzalez";
			customer.Phone = "343-343-343"; //INVALID VALUE
			customer.Zip = "34334"; //INVALID VALUE

			using (ISession session = sf.OpenSession())
			{
				try
				{
					using (ITransaction tx = session.BeginTransaction())
					{
						session.Save(customer);
						session.Flush();
						tx.Commit();
					}
				}
				catch (InvalidStateException ex)
				{
					Console.WriteLine("Everything is ok, the object wasn't saved ;)");
					PrintErrors(ex.GetInvalidValues());
				}
			}

			sf.Dispose();
		}

		/// <summary>
		/// On this example the entity pass through validation, and NHibernate
		/// persist the new object.
		/// </summary>
		private static void SavingAValidEntity()
		{
			Console.WriteLine("======================================");
			Console.WriteLine("Example: Saving a valid entity");
			Console.WriteLine("======================================");

			//NHibernate configuration: see hibernate.cfg.xml
			Configuration cfg = new Configuration();
			cfg.Configure();

			//NHibernate Validator configuration: see nhvalidator.cfg.xml
			ValidatorEngine engine = new ValidatorEngine();
			engine.Configure(); //by convention reads the nhvalidator.cfg.xml file.

			//Registering of Listeners and DDL-applying here
			ValidatorInitializer.Initialize(cfg, engine);

			ISessionFactory sf = cfg.BuildSessionFactory();

			Customer customer = new Customer();
			customer.Born = DateTime.Parse("1/1/1982");
			customer.Email = "antonio@mail.com";
			customer.FirstName = "Antonio";
			customer.LastName = "Gonzalez";
			customer.Phone = "333-444-5555";
			customer.Zip = "2ADG-78DB";

			using (ISession session = sf.OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					int IdGenerated = -1;

					IdGenerated = (int)session.Save(customer);
					session.Flush();
					tx.Commit();

					Console.WriteLine("The Id of the valid entity saved it's: {0}",IdGenerated);
				}
			}

			sf.Dispose();
		}

		private static void PrintErrors(InvalidValue[] values)
		{
			foreach (InvalidValue value in values)
			{
				Console.WriteLine("\t-Invalid Property: {0}. Error Message: {1}", value.PropertyName, value.Message);
			}
		}
	}
}