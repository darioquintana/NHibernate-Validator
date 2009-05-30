using System;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.GraphNavigation
{
	public class Address
	{
		[NotNullNotEmpty]
		[Length(Max = 30)]
		private string addressline1;

		private string zipCode;

		[Length(Max = 30)] [NotNullNotEmpty] private String city;

		[Valid] private User inhabitant;

		public Address()
		{
		}

		public Address(string addressline1, string zipCode, string city)
		{
			this.addressline1 = addressline1;
			this.zipCode = zipCode;
			this.city = city;
		}

		public String getAddressline1()
		{
			return addressline1;
		}

		public void setAddressline1(String addressline1)
		{
			this.addressline1 = addressline1;
		}

		public String getZipCode()
		{
			return zipCode;
		}

		public void setZipCode(String zipCode)
		{
			this.zipCode = zipCode;
		}

		public String getCity()
		{
			return city;
		}

		public void setCity(String city)
		{
			this.city = city;
		}

		public User getInhabitant()
		{
			return inhabitant;
		}

		public void SetInhabitant(User inhabitant)
		{
			this.inhabitant = inhabitant;
		}
	}
}