using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.GraphNavigation
{
	public class OrderLine
	{
		public OrderLine(Order order, int articleNumber)
		{
			ArticleNumber = articleNumber;
			Order = order;
		}

		[Valid]
		public Order Order { get; set; }

		[NotNull]
		public int? ArticleNumber { get; set; }
	}
}