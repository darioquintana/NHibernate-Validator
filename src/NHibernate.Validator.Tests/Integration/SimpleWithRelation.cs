namespace NHibernate.Validator.Tests.Integration
{
	public class SimpleWithRelation
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual Relation Relation { get; set; }
	}

	public class Relation
	{
		public virtual int Id { get; set; }
		public virtual string Description { get; set; }
	}
}