using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Inheritance
{
    using System;

    public interface IBoneEater : IEater
    {
        [NotNull]
        String FavoriteBone { get; set; }
    }

	public class IBoneEaterDef: ValidationDef<IBoneEater>
	{
		public IBoneEaterDef()
		{
			Define(x => x.FavoriteBone).NotNullable();
		}
	}
}