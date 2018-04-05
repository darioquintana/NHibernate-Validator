using System;
using System.Collections.Generic;

namespace NHibernate.Validator.Tests.Integration
{
	public class FromNhibMetadata
	{
		public virtual int Id { get; set; }

		public virtual DateTime? DateNotNull { get; set; }

		public virtual string StrValue { get; set; }

		public virtual decimal Dec { get; set; }

		public virtual En1 EnumV { get; set; }

		public virtual Cmp1 Cmp { get; set; }

		public virtual ISet<Cmp2> Cmps2
		{
			get { return _cmps2; }
			set { _cmps2 = value; }
		}
		private ISet<Cmp2> _cmps2 = new HashSet<Cmp2>();
	}

	public abstract class Cmp1Base
	{
		public virtual FromNhibMetadata Container { get; set; }
		public virtual En1 CEnumV { get; set; }
		public virtual string CStrValue { get; set; }
	}

	public class Cmp1 : Cmp1Base
	{ }

	public class Cmp2
	{
		public virtual En1 CEnumV1 { get; set; }
		public virtual string CStrValue1 { get; set; }

		protected virtual bool Equals(Cmp2 obj)
		{
			return CEnumV1 == obj.CEnumV1 && CStrValue1 == obj.CStrValue1;
		}

		public override int GetHashCode()
		{
			return (CStrValue1 + CEnumV1.ToString()).GetHashCode();
		}

	}

	public enum En1
	{
		v1 = 0,
		v2 = 1
	}
}
