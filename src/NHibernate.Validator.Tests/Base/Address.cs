using System;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Base
{
	public class Address
	{
		[NotNull]
		public static string blacklistedZipCode;

		[Length(Max = 20), NotNull]
		private string country;

		[Range(Min = -2, Max = 50, Message = "{floor.out.of.range} (escaping #{el})")] 
		public int floor;

		private long id;
		private bool internalValid = true;
		private string line1;
		private string line2;
		private string state;
		private string zip;
		private AddressType addressType = AddressType.Mailing;
		private AddressFlag addressFlags = AddressFlag.Flag0; 

		[Min(1), Range(Max = 2000), Max(2500)]
		public long Id
		{
			get { return id; }
			set { id = value; }
		}

		public string Country
		{
			get { return country; }
			set { country = value; }
		}

		[NotNull]
		public string Line1
		{
			get { return line1; }
			set { line1 = value; }
		}

		[Length(Max = 3), NotNull]
		public string State
		{
			get { return state; }
			set { state = value; }
		}

		[Length(Max = 5, Message = "{long}")]
		[Pattern(Regex = "[0-9]+")]
		[NotNull]
		public string Zip
		{
			get { return zip; }
			set { zip = value; }
		}

		public string Line2
		{
			get { return line2; }
			set { line2 = value; }
		}

		[AssertTrue]
		public bool InternalValid
		{
			get { return internalValid; }
			set { internalValid = value; }
		}

		[Enum]
		public AddressType AddressType
		{
			get { return addressType; }
			set { addressType = value; }
		}

		[Enum]
		public AddressFlag AddressFlags
		{
			get { return addressFlags; }
			set { addressFlags = value; }
		}
	}

	public enum AddressType : byte
	{
		Home = 0,
		Mailing = 1
	}

	[Flags] 
	public enum AddressFlag : byte
	{
		Flag0 = 0,
		Flag1 = 1,
		Flag2 = 2,
		Flag4 = 4
	}


	public class AddressDef: ValidationDef<Address>
	{
		public AddressDef()
		{
			Define(x => x.Country)
				.MaxLength(20).And
				.NotNullable();
			Define(x => x.floor)
				.IncludedBetween(-2, 50).WithMessage("{floor.out.of.range} (escaping #{el})");
			Define(x => x.Id)
				.IncludedBetween(1, 2000);
			Define(x => x.Line1)
				.NotNullable();
			Define(x => x.State)
				.NotNullable().And
				.MaxLength(3);
			Define(x => x.Zip)
				.NotNullable().And
				.MaxLength(5).WithMessage("{long}").And
				.MatchWith("[0-9]+");
			Define(x => x.InternalValid)
				.IsTrue();
			Define(x => x.AddressType).Enum();
			Define(x => x.AddressFlags).Enum();
		}
	}
}