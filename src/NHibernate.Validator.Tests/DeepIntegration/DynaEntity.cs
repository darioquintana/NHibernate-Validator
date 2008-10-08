using System.Collections;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.DeepIntegration
{
	public class DynaEntity
	{
		private IDictionary dynaBean = new Hashtable(5);
		private int id;

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		[Valid]
		public virtual IDictionary DynaBean
		{
			get { return dynaBean; }
			set { dynaBean = value; }
		}
	}
}