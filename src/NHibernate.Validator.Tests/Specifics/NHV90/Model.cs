using System.Collections.Generic;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Specifics.NHV90
{
	public class TheParent
	{
		protected TheParent()
		{
		}

		public TheParent(string name)
		{
			Children = new List<TheChild>();
			Name = name;
		}

		public virtual long Id { get; set; }

		[Size(Min = 1)]
		public virtual IList<TheChild> Children { get; set; }

		public virtual string Name { get; set; }
	}

	public class TheChild
	{
		protected TheChild()
		{
		}

		public TheChild(string name)
		{
			Name = name;
		}

		public virtual long Id { get; set; }
		public virtual TheParent Parent { get; set; }

		public virtual string Name { get; set; }
	}
}