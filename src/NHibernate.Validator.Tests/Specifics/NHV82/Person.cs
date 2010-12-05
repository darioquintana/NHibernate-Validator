using NHibernate.Validator.Cfg.Loquacious;

namespace NHibernate.Validator.Tests.Specifics.NHV82
{
	public class Person
	{
		public string SomethingElse { get; set; }
		public Name Name { get; set; }
	}

	public class Name
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	public class NameValidation: ValidationDef<Name>
	{
		public NameValidation()
		{
			Define(name => name.FirstName).MaxLength(20);
			Define(name => name.LastName).MaxLength(35);
		}
	}

	public class PersonValidation : ValidationDef<Person>
	{
		public PersonValidation()
		{
			Define(person => person.SomethingElse).MaxLength(30);
			Define(person => person.Name).IsValid();
		}
	}
}