using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Constraints;

namespace NHibernate.Validator.Tests.Inheritance
{
    public class Dog : Animal, IBoneEater
    {
        private string favoriteBone;
        private int frequency;

        [Length(Min = 3)]
        public string FavoriteBone
        {
            get { return favoriteBone; }
            set { favoriteBone = value; }
        }

        public int Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }
    }

	public class DogDef: ValidationDef<Dog>
	{
		public DogDef()
		{
			Define(x => x.FavoriteBone).MinLength(3);
		}
	}
}