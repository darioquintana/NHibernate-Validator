namespace NHibernate.Validator.Tests.Specifics.NHV91
{
	public class EntityWithComponent
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual Component Component { get; set; }
	}

	public class Component
	{
		public virtual int Value { get; set; }
		public virtual NestedComponent NestedComponent { get; set; }
	}

	public class NestedComponent
	{
		public virtual string Description { get; set; }
	}
}