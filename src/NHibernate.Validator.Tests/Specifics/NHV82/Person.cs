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
	public class Supername
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Name Name { get; set; }
	}

	public class Pperson
	{
		public Supername SuperName { get; set; }
	}

	public class NameValidation: ValidationDef<Name>
	{
		public NameValidation()
		{
			Define(name => name.FirstName).MaxLength(20);
			Define(name => name.LastName).MaxLength(35);
		}
	}
	public class SupernameValidation : ValidationDef<Supername>
	{
		public SupernameValidation()
		{
			Define(name => name.FirstName).MaxLength(200);
			Define(name => name.LastName).MaxLength(350);
			Define(person => person.Name).IsValid();
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

	public class PpersonValidation : ValidationDef<Pperson>
	{
		public PpersonValidation()
		{
			Define(person => person.SuperName).IsValid();
		}
	}
}