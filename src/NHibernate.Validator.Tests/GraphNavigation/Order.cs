using System.Collections.Generic;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.GraphNavigation
{
	public class Order
	{
		[Valid] 
		private readonly List<OrderLine> orderLines = new List<OrderLine>();

		[NotNull] private int? orderId;

		public Order(int id)
		{
			orderId = id;
		}

		[Valid]
		public User Customer { get; set; }

		[Valid]
		public Address ShippingAddress { get; set; }

		[Valid]
		public Address BillingAddress { get; set; }

		public List<OrderLine> Orders
		{
			get { return orderLines; }
		}

		public void AddOrderLine(OrderLine orderLine)
		{
			orderLines.Add(orderLine);
		}
	}
}