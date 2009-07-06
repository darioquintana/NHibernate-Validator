using System.Diagnostics;
using Castle.Windsor;
using NHibernate.Validator.Demo.IoC.Windsor.MyValidators;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Demo.IoC.Windsor
{
	public class Program
	{
		private static void Main(string[] args)
		{
			///Configuration at Application Start-up.
			var container = new WindsorContainer();
			IoC.Container = container;
			//registering NHV out-of-the-box constraint validators
			WindsorConstraintValidatorFactory.RegisteringValidators(container);

			//registering your own constraint validators
			container.AddComponent(typeof (PersonNameValidator).Name.ToLower(), typeof (PersonNameValidator));
			///End of IoC Configuration.
			
			var vtor = new ValidatorEngine();
			vtor.Configure();
			
			//Validating a valid entity 'Contact'
			var validCustomer = new Contact
			                    	{
			                    		FirstName = "Dario",
			                    		LastName = "Quintana",
			                    		Address = "2nd Street at 34",
			                    		Description = "Some description"
			                    	};

			vtor.AssertValid(validCustomer);


			//Validating an invalid entity 'Contact'
			var invalidCustomer = new Contact
			                      	{
			                      		FirstName = "dario" /*INVALID*/, 
			                      		LastName = "Quintana",
			                      		Address = "2nd Street at 34",
			                      		Description = "Some description"
			                      	};

			Debug.Assert(vtor.Validate(invalidCustomer).Length == 1);
		}
	}
}