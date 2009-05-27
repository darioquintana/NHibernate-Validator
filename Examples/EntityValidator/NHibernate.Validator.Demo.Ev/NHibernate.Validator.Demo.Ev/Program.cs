using System;
using NHibernate.Validator.Demo.Ev.Model;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Demo.Ev
{
	public class Program
	{
		private static void Main(string[] args)
		{
			//Note: you have to choose one way to configure NHibernate.Validator. Comment/un-comment according what you prefer.
			// 1) Attributes
			// 2) Xml
			// 3) Fluent

			//Using Attributes
			var vtor = new ValidatorEngine(); //NHibernate Validator is prepared to configure with Attributes by default.

			//Using Xml
			//var vtor = Helper.Get_Engine_Configured_for_Xml();

			//Using Fluent
			//var vtor = Helper.Get_Engine_Configured_for_Fluent();
			
            //Running the example using the configuration chosen
			Run_Example_With_Valid_Entity(vtor);
			Run_Example_With_Invalid_Entity(vtor);

			Console.WriteLine("Done.");
			Console.ReadKey(true);
		}

		private static void Run_Example_With_Invalid_Entity(ValidatorEngine vtor)
		{
			Console.WriteLine("==Entity with Invalid values==");
			var m = new Meeting
			        	{
			        		Name = "NHibernate Validator virtual meeting",
			        		Description = "How to configure NHibernate Validator in different ways.",
			        		Start = DateTime.Now.AddHours(2), //Invalid: the Start date is minor than End.
							End = DateTime.Now
			        	};

			var invalidValues = vtor.Validate(m);
			if (invalidValues.Length == 0)
				throw new Exception("Something was wrong in the NHV configuration, the entity should be invalid.");
			else
			{
				Console.WriteLine("The entity is invalid.");
				foreach (var value in invalidValues)
				{
					Console.WriteLine(" - " + value.Message);
				}
			}	

		}

		private static void Run_Example_With_Valid_Entity(ValidatorEngine vtor)
		{
			Console.WriteLine("==Entity with valid values==");
			var m = new Meeting
			        	{
			        		Name = "NHibernate Validator virtual meeting",
			        		Description = "How to configure NHibernate Validator in different ways.",
			        		Start = DateTime.Now,
			        		End = DateTime.Now.AddHours(2) // 2 hours of meeting.
			        	};

			if(vtor.IsValid(m))
				Console.WriteLine("The entity is valid.");
			else
				throw new Exception("Something was wrong in the NHV configuration, the entity should be invalid.");
		}
	}
}