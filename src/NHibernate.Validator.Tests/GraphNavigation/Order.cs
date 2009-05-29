using System.Collections.Generic;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.GraphNavigation
{
	public class Order
	{
		[NotNull] private int orderId;

		[Valid] private List<OrderLine> orderLines = new List<OrderLine>();

		[Valid] private User customer;

		[Valid] private Address shippingAddress;

		[Valid] private Address billingAddress;

		public Order(int id)
		{
			orderId = id;
		}

		public void addOrderLine(OrderLine orderLine)
		{
			orderLines.Add(orderLine);
		}

		public List<OrderLine> getOrderLines()
		{
			return orderLines;
		}

		public User getCustomer()
		{
			return customer;
		}

		public void setCustomer(User customer)
		{
			this.customer = customer;
		}

		public Address getShippingAddress()
		{
			return shippingAddress;
		}

		public void setShippingAddress(Address shippingAddress)
		{
			this.shippingAddress = shippingAddress;
		}

		public Address getBillingAddress()
		{
			return billingAddress;
		}

		public void setBillingAddress(Address billingAddress)
		{
			this.billingAddress = billingAddress;
		}
	}
}