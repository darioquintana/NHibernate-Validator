using System.Collections.Generic;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Specifics.NHV90
{
	public class TheParent
	{
		protected TheParent()
		{
			children = new List<TheChild>();
		}

		public TheParent(string name):this()
		{
			Name = name;
		}

		public virtual long Id { get; set; }

		private IList<TheChild> children;

		[Size(Min = 1)]
		public virtual IEnumerable<TheChild> Children
		{
			get { return children; }
		}

		public virtual void AddChild(TheChild child)
		{
			child.Parent = this;
			children.Add(child);
		}
		public virtual void ClearChildren()
		{
			foreach (var child in children)
			{
				child.Parent = null;
			}
			children.Clear();
		}
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