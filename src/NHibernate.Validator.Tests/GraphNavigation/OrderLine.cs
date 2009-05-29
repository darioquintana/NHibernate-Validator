using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.GraphNavigation
{
	public class OrderLine
	{
		[NotNull] private int articleNumber;

		[Valid] private Order order;

		public OrderLine(Order order, int articleNumber)
		{
			this.articleNumber = articleNumber;
			this.order = order;
		}

		public int getArticleNumber()
		{
			return articleNumber;
		}

		public void setArticleNumber(int articleNumber)
		{
			this.articleNumber = articleNumber;
		}
	}
}